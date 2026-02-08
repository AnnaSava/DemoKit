using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates how an in-process worker produces high-frequency output.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>spam</c> mode,
/// where it writes messages to the console continuously.
/// The worker runs until the user cancels the demo (for example, Ctrl+C).
/// </remarks>
public sealed class SpamScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Spam";

    /// <summary>
    /// Runs the in-process worker spam-output scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (spam mode)...");
        Console.WriteLine();

        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "spam", "--interval", "50" });

        await worker.RunAsync();
    }
}
