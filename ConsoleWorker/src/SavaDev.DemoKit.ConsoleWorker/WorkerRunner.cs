using SavaDev.DemoKit.ConsoleWorker.Args;
using SavaDev.DemoKit.ConsoleWorker.Executor;
using SavaDev.DemoKit.ConsoleWorker.Helpers;

namespace SavaDev.DemoKit.ConsoleWorker;

/// <summary>
/// Coordinates a single execution of the demo worker.
/// </summary>
/// <remarks>
/// This class represents the top-level execution flow of the worker.
/// It is responsible for wiring together parsed arguments, configuration,
/// output handling, and the selected worker mode.
///
/// The runner deliberately focuses on orchestration rather than behavior:
/// it does not implement worker logic itself, but instead delegates
/// execution to a mode executor while managing startup, cancellation,
/// and shutdown concerns.
///
/// In other words, this is the part that makes sure everything starts,
/// runs, and stops in the correct order -- even when a human presses Ctrl+C
/// at an inconvenient moment.
/// </remarks>
internal sealed class WorkerRunner
{
    private readonly ConsoleWorkerOptions _options;
    private readonly ArgsOptions _argsOptions;
    private readonly ConsoleOutputHelper _output;
    private readonly IWorkerModeExecutor _executor;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkerRunner"/> class
    /// with explicitly provided dependencies.
    /// </summary>
    /// <param name="options">Worker configuration.</param>
    /// <param name="argsOptions">Parsed start options.</param>
    /// <param name="output">Output helper used to write console messages.</param>
    /// <param name="executor">Executor responsible for running the selected worker mode.</param>
    internal WorkerRunner(
        ConsoleWorkerOptions options,
        ArgsOptions argsOptions,
        ConsoleOutputHelper output,
        IWorkerModeExecutor? executor)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _argsOptions = argsOptions ?? throw new ArgumentNullException(nameof(argsOptions));
        _output = output ?? throw new ArgumentNullException(nameof(output));
        _executor = executor ?? new WorkerModeExecutor(_argsOptions, _options, _output);
    }

    /// <summary>
    /// Runs the worker execution flow.
    /// </summary>
    /// <returns>
    /// An integer exit code representing the outcome of the execution.
    /// </returns>
    /// <remarks>
    /// This method performs the complete lifecycle of a worker run:
    /// startup information is printed, cancellation handling is registered,
    /// and the selected worker mode is executed.
    ///
    /// Cancellation is handled cooperatively and is considered a normal
    /// completion path rather than an error condition.
    /// </remarks>
    public async Task<int> RunAsync()
    {
        PrintStartup();

        ConsoleCancelEventHandler? handler = null;
        try
        {
            using var cts = new CancellationTokenSource();
            handler = RegisterCancelKeyPressHandler(cts);
            return await ExecuteWithCancellationHandlingAsync(cts.Token);
        }
        finally
        {
            if (handler is not null)
            {
                Console.CancelKeyPress -= handler;
            }
        }
    }

    /// <summary>
    /// Prints initial startup information to the output.
    /// </summary>
    /// <remarks>
    /// The startup output is intentionally concise and human-readable.
    /// It provides enough context to understand what is running, how it
    /// was configured, and which mode was selected, without overwhelming
    /// the reader with details.
    /// </remarks>
    private void PrintStartup()
    {
        _output.WriteLine(_options.WorkerName);
        _output.WriteLine("Console demo worker started");
        _output.WriteLine($"Mode: {_argsOptions.Mode}");
        _output.WriteLine($"PID: {Environment.ProcessId}");
        _output.WriteLine();
    }

    /// <summary>
    /// Registers a handler for Ctrl+C to trigger cooperative cancellation.
    /// </summary>
    /// <param name="cts">
    /// A cancellation token source that will be cancelled when Ctrl+C is pressed.
    /// </param>
    /// <returns>
    /// The registered console cancel event handler.
    /// </returns>
    /// <remarks>
    /// The handler suppresses the default process termination behavior
    /// and instead signals cancellation through the provided token source.
    ///
    /// Late cancellation events, occurring after the worker has already
    /// completed, are safely ignored.
    /// </remarks>
    private static ConsoleCancelEventHandler RegisterCancelKeyPressHandler(
        CancellationTokenSource cts)
    {
        ConsoleCancelEventHandler handler = (_, e) =>
        {
            e.Cancel = true;
            try
            {
                cts.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // Ignore late Ctrl+C after the worker has already completed.
            }
        };

        Console.CancelKeyPress += handler;
        return handler;
    }

    /// <summary>
    /// Executes the worker while handling cooperative cancellation.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token used to observe cancellation requests.
    /// </param>
    /// <returns>
    /// An integer exit code representing the execution result.
    /// </returns>
    /// <remarks>
    /// This method delegates execution to the mode executor and converts
    /// cancellation into a graceful shutdown message and a successful exit code.
    ///
    /// From the runner’s perspective, cancellation is an expected and
    /// well-behaved outcome — not a failure.
    /// </remarks>
    private async Task<int> ExecuteWithCancellationHandlingAsync(
        CancellationToken ct)
    {
        try
        {
            return await _executor.ExecuteAsync(ct);
        }
        catch (OperationCanceledException)
        {
            _output.WriteLine();
            _output.WriteLine("Worker cancelled.");
            return 0;
        }
    }
}
