using SavaDev.DemoKit.ConsoleWorker.Args;
using SavaDev.DemoKit.ConsoleWorker.Helpers;

namespace SavaDev.DemoKit.ConsoleWorker;

/// <summary>
/// Represents a console-based demo worker with multiple execution modes.
/// </summary>
/// <remarks>
/// This worker executes simple, predefined behaviors selected via
/// command-line arguments. It is designed to be run either directly
/// by demo applications or invoked by test harnesses.
///
/// The worker focuses on producing observable and repeatable behavior,
/// such as long-running execution, cancellation, or failure, in order
/// to make execution flow and output handling easy to verify.
///
/// The implementation intentionally avoids business logic and keeps
/// behavior explicit and straightforward. The class is intended for
/// demo and testing scenarios only.
/// </remarks>
public sealed class ConsoleDemoWorker
{
    private int _hasRun;
    private readonly ConsoleWorkerOptions _options;
    private readonly string[] _args;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleDemoWorker"/> class.
    /// </summary>
    /// <param name="options">Optional worker configuration.</param>
    /// <param name="args">Optional command-line arguments.</param>
    public ConsoleDemoWorker(ConsoleWorkerOptions? options = null, string[]? args = null)
    {
        _options = options ?? new ConsoleWorkerOptions();
        _args = args ?? Array.Empty<string>();
    }

    /// <summary>
    /// Starts the worker and executes the selected mode based on the parsed arguments.
    /// </summary>
    /// <returns>
    /// An integer exit code indicating the result of execution.
    /// </returns>
    /// <remarks>
    /// This method may be called only once per worker instance.
    /// It parses command-line arguments, reports parsing warnings,
    /// initializes required helpers, and delegates execution to
    /// <see cref="WorkerRunner"/>.
    ///
    /// Argument parsing errors are reported to the console and result
    /// in a non-zero exit code. Cancellation is handled by the runner
    /// and is considered a normal completion path.
    /// </remarks>
    public async Task<int> RunAsync()
    {
        if (Interlocked.Exchange(ref _hasRun, 1) == 1)
        {
            throw new InvalidOperationException(
                "ConsoleDemoWorker can only be run once per instance.");
        }

        var parser = new ConsoleArgsParser();
        ArgsParseResult parseResult;
        try
        {
            parseResult = parser.Parse(_args);
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or ArgumentOutOfRangeException)
        {
            var fallbackOutput = new ConsoleOutputHelper(new ArgsOptions());
            fallbackOutput.WriteLine(ex.Message);
            return 1;
        }

        var output = new ConsoleOutputHelper(parseResult.Options);
        foreach (var warning in parseResult.Warnings)
        {
            output.WriteLine(warning);
        }

        var runner = new WorkerRunner(
            options: _options,
            argsOptions: parseResult.Options,
            output: output,
            executor: null);

        return await runner.RunAsync();
    }
}
