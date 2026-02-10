using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates run mode with customized heartbeat message formatting.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>run</c> mode with a custom
/// message template to show how output can be configured.
/// </remarks>
public sealed class RunCustomMessageScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Run (custom message)";

    /// <summary>
    /// Runs the scenario with a customized heartbeat message template.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (run mode, custom messages)...");
        Console.WriteLine();

        var options = new ConsoleWorkerOptions
        {
            RunMessageTemplate = "Tick #{0}"
        };

        var worker = new ConsoleDemoWorker(
            options: options,
            args: new[] { "--mode", "run", "--interval", "500" });

        await worker.RunAsync();
    }
}
