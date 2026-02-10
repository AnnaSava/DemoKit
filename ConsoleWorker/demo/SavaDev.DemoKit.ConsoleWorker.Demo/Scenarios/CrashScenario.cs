using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates how an in-process worker fails with a crash.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>crash</c> mode,
/// where it throws an exception and terminates execution.
/// </remarks>
public sealed class CrashScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Crash";

    /// <summary>
    /// Runs the in-process worker crash scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (crash mode)...");
        Console.WriteLine();

        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "crash" });

        try
        {
            await worker.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Worker crashed as expected.");
            Console.WriteLine(ex.GetType().Name);
            Console.WriteLine(ex.Message);
        }
    }
}
