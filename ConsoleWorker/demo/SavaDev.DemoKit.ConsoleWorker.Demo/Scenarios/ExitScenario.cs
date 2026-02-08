using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates how an in-process worker exits normally.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>exit</c> mode,
/// where it waits briefly and terminates with exit code <c>0</c>.
/// </remarks>
public sealed class ExitScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Exit";

    /// <summary>
    /// Runs the in-process worker normal-exit scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (exit mode)...");
        Console.WriteLine();

        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "exit", "--exit-after", "2" });

        var exitCode = await worker.RunAsync();

        Console.WriteLine();
        Console.WriteLine($"Worker exited with code {exitCode}.");
    }
}
