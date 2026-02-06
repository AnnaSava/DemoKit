namespace SavaDev.DemoKit.ConsoleEngine.Demo.Scenarios;

/// <summary>
/// Demonstrates a simple delayed execution scenario
/// using asynchronous waiting.
/// </summary>
/// <remarks>
/// This scenario waits for a fixed amount of time and then
/// completes without producing any intermediate output.
///
/// It is intended to illustrate:
/// <list type="bullet">
/// <item>How to perform asynchronous delays in a scenario.</item>
/// <item>How to correctly observe <see cref="CancellationToken"/>.</item>
/// <item>That a scenario may remain silent while doing work.</item>
/// </list>
///
/// The scenario can be cancelled at any time via Ctrl+C,
/// in which case the delay is interrupted immediately.
/// </remarks>
public sealed class SleepScenario : IConsoleDemoScenario
{
    /// <summary>
    /// The duration of the sleep period, in seconds.
    /// </summary>
    private const int SleepSeconds = 5;

    /// <inheritdoc />
    public string Name => "sleep";

    /// <summary>
    /// Runs the sleep demonstration scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the sleep operation.
    /// </param>
    /// <remarks>
    /// The method waits asynchronously for a fixed amount
    /// of time and then completes.
    ///
    /// If cancellation is requested during the wait,
    /// the operation is aborted immediately.
    /// </remarks>
    public async Task RunAsync(CancellationToken ct)
    {
        Console.WriteLine(
            $"Sleeping for {SleepSeconds} seconds...");
        Console.WriteLine("Press Ctrl+C to cancel.");
        Console.WriteLine();

        await Task.Delay(
            TimeSpan.FromSeconds(SleepSeconds),
            ct);

        Console.WriteLine();
        Console.WriteLine("Sleep completed.");
    }
}

