namespace SavaDev.DemoKit.ConsoleEngine;

/// <summary>
/// Represents a runnable demonstration scenario.
/// </summary>
/// <remarks>
/// A demo scenario encapsulates a single, self-contained
/// example that illustrates a specific usage pattern or
/// feature of a library.
///
/// Scenarios are intended to be executed by demo
/// applications and do not represent production code.
/// </remarks>
public interface IConsoleDemoScenario
{
    /// <summary>
    /// Gets the display name of the scenario.
    /// </summary>
    /// <remarks>
    /// The name is used for presenting the scenario
    /// in menus or console output.
    /// </remarks>
    string Name { get; }

    /// <summary>
    /// Executes the demonstration scenario.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous execution
    /// of the scenario.
    /// </returns>
    Task RunAsync(CancellationToken ct);
}
