using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates running a demo worker in-process
/// in a normal long-running mode.
/// </summary>
/// <remarks>
/// This scenario creates and runs a demo worker instance
/// in-process in <c>run</c> mode, where the worker produces
/// periodic output indefinitely.
///
/// This example demonstrates:
/// <list type="bullet">
/// <item>How to start a worker with predefined arguments.</item>
/// <item>How long-running output looks in an in-process demo.</item>
/// <item>How the worker terminates gracefully on Ctrl+C.</item>
/// </list>
/// </remarks>
public sealed class RunScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Worker run";

    /// <summary>
    /// Runs the worker execution scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to terminate
    /// the scenario execution.
    /// </param>
    /// <remarks>
    /// The scenario creates a <see cref="ConsoleDemoWorker"/> instance
    /// and executes it in <c>run</c> mode.
    ///
    /// The worker continues running until cancellation is requested,
    /// typically triggered by the user via Ctrl+C.
    /// The scenario itself does not pass the token to the worker,
    /// because the worker handles Ctrl+C internally.
    /// </remarks>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (run mode)...");
        Console.WriteLine();

        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "run" });

        Console.WriteLine("Worker started.");
        Console.WriteLine();

        try
        {
            await worker.RunAsync();
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine();
            Console.WriteLine("Stopping worker...");
        }
    }
}
