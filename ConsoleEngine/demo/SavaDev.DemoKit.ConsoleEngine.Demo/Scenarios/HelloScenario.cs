namespace SavaDev.DemoKit.ConsoleEngine.Demo.Scenarios;

/// <summary>
/// Demonstrates the simplest possible ConsoleKit scenario.
/// </summary>
/// <remarks>
/// This scenario prints a short greeting message to the console
/// and exits immediately.
///
/// It is intended as a minimal example that illustrates:
/// <list type="bullet">
/// <item>The structure of an <see cref="IConsoleDemoScenario"/> implementation.</item>
/// <item>How a scenario is named and executed.</item>
/// <item>That scenarios may complete synchronously without delays.</item>
/// </list>
///
/// The scenario does not perform any asynchronous work and does
/// not react to cancellation, as it completes instantly.
/// </remarks>
public sealed class HelloScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "hello";

    /// <summary>
    /// Runs the hello demonstration scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token provided by the host.
    /// </param>
    /// <remarks>
    /// The cancellation token is not observed because the
    /// scenario completes immediately.
    /// </remarks>
    public Task RunAsync(CancellationToken ct)
    {
        Console.WriteLine("Hello from ConsoleKit!");
        Console.WriteLine("This is the simplest possible demo scenario.");

        return Task.CompletedTask;
    }
}
