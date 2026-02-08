using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates the in-process worker echo mode.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>echo</c> mode, where
/// it reads user input and prints it back until cancellation.
/// </remarks>
public sealed class EchoScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Echo";

    /// <summary>
    /// Runs the in-process worker echo scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (echo mode)...");
        Console.WriteLine();

        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "echo" });

        await worker.RunAsync();
    }
}
