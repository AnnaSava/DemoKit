using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates the in-process worker art mode.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>art</c> mode,
/// which renders an ASCII art banner until cancellation.
/// </remarks>
public sealed class ArtScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Art";

    /// <summary>
    /// Runs the in-process worker art scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (art mode)...");
        Console.WriteLine();

        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "art" });

        await worker.RunAsync();
    }
}
