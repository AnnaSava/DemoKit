namespace SavaDev.DemoKit.ConsoleEngine.Demo.Scenarios;

/// <summary>
/// Demonstrates a simple countdown scenario using ConsoleKit.
/// </summary>
/// <remarks>
/// This scenario performs a countdown from a predefined number
/// to zero, printing one line per second.
///
/// It is intentionally minimal and self-contained to illustrate:
/// <list type="bullet">
/// <item>How to implement <see cref="IConsoleDemoScenario"/>.</item>
/// <item>How to perform asynchronous work in a scenario.</item>
/// <item>How to correctly observe <see cref="CancellationToken"/>.</item>
/// </list>
///
/// The scenario can be cancelled at any time (for example, via Ctrl+C),
/// in which case the countdown stops gracefully.
/// </remarks>
public sealed class CountdownScenario : IConsoleDemoScenario
{
    /// <summary>
    /// The number of seconds to count down from.
    /// </summary>
    private const int CountdownSeconds = 5;

    /// <inheritdoc />
    public string Name => "Countdown";

    /// <summary>
    /// Runs the countdown scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the countdown execution.
    /// </param>
    /// <remarks>
    /// The countdown prints one message per second and completes
    /// normally when it reaches zero.
    ///
    /// If cancellation is requested before completion, the method
    /// stops immediately and propagates the cancellation signal.
    /// </remarks>
    public async Task RunAsync(CancellationToken ct)
    {
        Console.WriteLine("Starting countdown...");
        Console.WriteLine();

        for (var i = CountdownSeconds; i > 0; i--)
        {
            ct.ThrowIfCancellationRequested();

            Console.WriteLine($"{i}...");
            await Task.Delay(TimeSpan.FromSeconds(1), ct);
        }

        Console.WriteLine();
        Console.WriteLine("Countdown completed.");
    }
}
