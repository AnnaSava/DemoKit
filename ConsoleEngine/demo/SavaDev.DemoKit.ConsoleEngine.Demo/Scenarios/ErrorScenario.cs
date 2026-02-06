namespace SavaDev.DemoKit.ConsoleEngine.Demo.Scenarios;

/// <summary>
/// Demonstrates how ConsoleKit behaves when a scenario
/// throws an unhandled exception.
/// </summary>
/// <remarks>
/// This scenario intentionally throws an exception after
/// a short delay.
///
/// It is designed to illustrate:
/// <list type="bullet">
/// <item>How errors inside a scenario propagate.</item>
/// <item>How ConsoleKit reports failed scenarios.</item>
/// <item>What kind of output a user sees when a scenario crashes.</item>
/// </list>
///
/// The scenario does not attempt to handle or recover from
/// the error. The exception is thrown deliberately to keep
/// the behavior explicit and easy to reason about.
/// </remarks>
public sealed class ErrorScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Error";

    /// <summary>
    /// Runs the error demonstration scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution before the error occurs.
    /// </param>
    /// <remarks>
    /// The method waits briefly to simulate some ongoing work
    /// and then throws an <see cref="InvalidOperationException"/>.
    ///
    /// If cancellation is requested before the exception is thrown,
    /// the cancellation is observed and propagated instead.
    /// </remarks>
    public async Task RunAsync(CancellationToken ct)
    {
        Console.WriteLine("Starting error scenario...");
        Console.WriteLine("An exception will be thrown shortly.");
        Console.WriteLine();

        await Task.Delay(TimeSpan.FromSeconds(2), ct);

        throw new InvalidOperationException(
            "This exception is thrown intentionally by the ErrorScenario.");
    }
}
