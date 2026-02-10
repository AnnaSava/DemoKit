using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates run mode with cyan-colored output.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>run</c> mode and sets
/// <c>--color cyan</c> to show colored console output.
/// </remarks>
public sealed class RunCyanScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Run (cyan)";

    /// <summary>
    /// Runs the scenario with cyan output.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (run mode, cyan output)...");
        Console.WriteLine();

        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "run", "--interval", "500", "--color", "cyan" });

        await worker.RunAsync();
    }
}
