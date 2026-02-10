using SavaDev.DemoKit.ConsoleWorker.Args;
using SavaDev.DemoKit.ConsoleWorker.Helpers;

namespace SavaDev.DemoKit.ConsoleWorker.Executor;

/// <summary>
/// Executes a selected worker mode based on parsed command-line options.
/// </summary>
/// <remarks>
/// This class represents a small mode dispatcher used in demo and testing scenarios.
/// It interprets the already parsed <see cref="ArgsOptions"/> and invokes the
/// corresponding worker behavior.
///
/// The executor itself does not perform argument parsing and does not manage
/// process lifetime or hosting concerns. Its sole responsibility is to
/// translate a selected mode into executable behavior.
///
/// This separation keeps worker logic explicit, predictable, and easy to
/// exercise in demos, tests, and process orchestration examples.
/// </remarks>
internal sealed class WorkerModeExecutor : IWorkerModeExecutor
{
    private readonly ArgsOptions _argsOptions;
    private readonly ConsoleWorkerOptions _workerOptions;
    private readonly ConsoleOutputHelper _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkerModeExecutor"/> class.
    /// </summary>
    /// <param name="argsOptions">Start options.</param>
    /// <param name="workerOptions">Worker configuration.</param>
    /// <param name="output">Output helper.</param>
    public WorkerModeExecutor(
        ArgsOptions argsOptions,
        ConsoleWorkerOptions workerOptions,
        ConsoleOutputHelper output)
    {
        _argsOptions = argsOptions ?? throw new ArgumentNullException(nameof(argsOptions));
        _workerOptions = workerOptions ?? throw new ArgumentNullException(nameof(workerOptions));
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    /// <summary>
    /// Executes a specific worker mode.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process exit code.</returns>
    public async Task<int> ExecuteAsync(CancellationToken ct)
    {
        return _argsOptions.Mode switch
        {
            WorkerMode.Run => await RunAsync(ct),
            WorkerMode.Exit => await ExitAsync(ct),
            WorkerMode.Crash => Crash(),
            WorkerMode.Spam => await SpamAsync(ct),
            WorkerMode.Echo => await EchoAsync(ct),
            WorkerMode.Art => await Art(ct),
            _ => throw new InvalidOperationException("Unknown mode.")
        };
    }

    /// <summary>
    /// Runs an infinite loop with periodic heartbeat output.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process exit code.</returns>
    private async Task<int> RunAsync(CancellationToken ct)
    {
        _output.PrintExitHint();

        var tick = 0;

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            tick++;
            _output.WriteLine(string.Format(
                _workerOptions.RunMessageTemplate,
                tick));
            await Task.Delay(_argsOptions.Interval, ct);
        }
    }

    /// <summary>
    /// Waits for the configured delay and exits.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process exit code.</returns>
    private async Task<int> ExitAsync(CancellationToken ct)
    {
        _output.WriteLine(
            $"Worker will exit in {_argsOptions.ExitAfterSeconds} seconds.");

        await Task.Delay(
            TimeSpan.FromSeconds(_argsOptions.ExitAfterSeconds),
            ct);

        _output.WriteLine("Exiting normally.");
        return 0;
    }

    /// <summary>
    /// Intentionally throws an exception to demonstrate a crash.
    /// </summary>
    /// <returns>Process exit code.</returns>
    /// <exception cref="InvalidOperationException">
    /// Always thrown to simulate a crash.
    /// </exception>
    private int Crash()
    {
        _output.WriteLine("Crashing now...");
        throw new InvalidOperationException(
            "Intentional crash requested by --mode crash.");
    }

    /// <summary>
    /// Continuously prints spam messages with the configured interval.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process exit code.</returns>
    private async Task<int> SpamAsync(CancellationToken ct)
    {
        _output.PrintExitHint();

        var i = 0;

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            i++;
            _output.WriteLine(string.Format(
                _workerOptions.SpamMessageTemplate,
                i));
            await Task.Delay(_argsOptions.Interval, ct);
        }
    }

    /// <summary>
    /// Executes the art mode banner output.
    /// </summary>
    /// <returns>Process exit code.</returns>
    private async Task<int> Art(CancellationToken ct)
    {
        _output.PrintExitHint();

        await Ascii.Art.DrawOnScreen(ct);

        return 0;
    }

    /// <summary>
    /// Reads input lines and echoes them until cancelled.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process exit code.</returns>
    private async Task<int> EchoAsync(CancellationToken ct)
    {
        _output.WriteLine("Echo mode: type a line and press Enter (Ctrl+C to stop).");

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            var readTask = Console.In.ReadLineAsync(CancellationToken.None).AsTask();
            var completed = await Task.WhenAny(
                readTask,
                Task.Delay(Timeout.InfiniteTimeSpan, ct));

            if (!ReferenceEquals(completed, readTask))
            {
                ct.ThrowIfCancellationRequested();
                continue;
            }

            var line = await readTask;

            if (line is null)
            {
                await Task.Yield();
                continue;
            }

            _output.WriteLine(line);
        }
    }
}
