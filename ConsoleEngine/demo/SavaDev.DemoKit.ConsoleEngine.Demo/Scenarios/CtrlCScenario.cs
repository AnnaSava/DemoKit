namespace SavaDev.DemoKit.ConsoleEngine.Demo.Scenarios;

/// <summary>
/// Demonstrates how ConsoleKit handles user-initiated
/// cancellation via Ctrl+C.
/// </summary>
/// <remarks>
/// This scenario runs an infinite loop that periodically
/// prints a heartbeat message to the console.
///
/// It relies entirely on <see cref="CancellationToken"/>
/// provided by the host and does not initiate cancellation
/// on its own.
///
/// The scenario is intended to demonstrate:
/// <list type="bullet">
/// <item>How Ctrl+C is propagated to scenarios.</item>
/// <item>How a scenario should observe cancellation.</item>
/// <item>How graceful shutdown looks from user perspective.</item>
/// </list>
///
/// The scenario exits cleanly when cancellation is requested,
/// without throwing additional exceptions.
/// </remarks>
public sealed class CtrlCScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Ctrl+C";

    /// <summary>
    /// Runs the Ctrl+C demonstration scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that is cancelled when the user
    /// presses Ctrl+C in the console.
    /// </param>
    /// <remarks>
    /// The method loops indefinitely, emitting a message
    /// every second, until cancellation is requested.
    ///
    /// When cancellation occurs, the method exits gracefully
    /// and allows the host to handle shutdown logic.
    /// </remarks>
    public async Task RunAsync(CancellationToken ct)
    {
        Console.WriteLine("Ctrl+C scenario started.");
        Console.WriteLine("Press Ctrl+C to cancel.");
        Console.WriteLine();

        var counter = 0;

        try
        {
            while (true)
            {
                ct.ThrowIfCancellationRequested();

                counter++;
                Console.WriteLine($"Running... tick {counter}");

                await Task.Delay(TimeSpan.FromSeconds(1), ct);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine();
            Console.WriteLine("Cancellation requested (Ctrl+C).");
            Console.WriteLine("Shutting down scenario gracefully.");
        }
    }
}
