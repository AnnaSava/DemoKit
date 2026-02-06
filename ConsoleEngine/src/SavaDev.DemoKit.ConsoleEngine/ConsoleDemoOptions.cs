namespace SavaDev.DemoKit.ConsoleEngine;

/// <summary>
/// Defines configurable text and behavior for the console demo engine.
/// </summary>
public sealed class ConsoleDemoOptions
{
    /// <summary>
    /// Gets the demo title shown in the header.
    /// </summary>
    public string Title { get; init; } = "Demo Scenarios";

    /// <summary>
    /// Gets the header border line displayed above and below the title.
    /// </summary>
    public string HeaderBorderLine { get; init; } = "========================================";

    /// <summary>
    /// Gets the separator line used for scenario headers and footers.
    /// </summary>
    public string SeparatorLine { get; init; } = "----------------------------------------";

    /// <summary>
    /// Gets the prompt displayed before reading menu input.
    /// </summary>
    public string MenuPrompt { get; init; } = "Select a scenario: ";

    /// <summary>
    /// Gets the selection key used to run all scenarios.
    /// </summary>
    public string RunAllKey { get; init; } = "A";

    /// <summary>
    /// Gets the label shown for the run-all option.
    /// </summary>
    public string RunAllLabel { get; init; } = "Run all scenarios";

    /// <summary>
    /// Gets the selection key used to quit the demo.
    /// </summary>
    public string QuitKey { get; init; } = "Q";

    /// <summary>
    /// Gets the label shown for the quit option.
    /// </summary>
    public string QuitLabel { get; init; } = "Quit";

    /// <summary>
    /// Gets the message displayed for invalid menu input.
    /// </summary>
    public string InvalidSelectionMessage { get; init; } = "Invalid selection.";

    /// <summary>
    /// Gets the message displayed when a scenario is cancelled.
    /// </summary>
    public string ScenarioCancelledMessage { get; init; } = "Scenario cancelled.";

    /// <summary>
    /// Gets the header shown when an unexpected error occurs.
    /// </summary>
    public string UnexpectedErrorHeader { get; init; } = "An unexpected error occurred:";

    /// <summary>
    /// Gets the format string used in the scenario header.
    /// </summary>
    public string ScenarioHeaderFormat { get; init; } = " Running scenario: {0}";

    /// <summary>
    /// Gets the text displayed in the scenario footer.
    /// </summary>
    public string ScenarioFooterTitle { get; init; } = " Scenario completed";

    /// <summary>
    /// Gets the prompt displayed before pausing for a key press.
    /// </summary>
    public string PausePrompt { get; init; } = "Press any key to continue...";

    /// <summary>
    /// Gets a value indicating whether the console should be cleared
    /// when printing the header.
    /// </summary>
    public bool ClearScreenOnHeader { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the engine should pause
    /// after each batch of executed scenarios.
    /// </summary>
    public bool PauseAfterScenarios { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the engine should handle
    /// <see cref="Console.CancelKeyPress"/> to cancel the current scenario.
    /// </summary>
    public bool HandleCancelKeyPress { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether pressing Ctrl+C
    /// while no scenario is running should terminate the process.
    /// </summary>
    /// <remarks>
    /// When enabled, the demo engine will call <see cref="Environment.Exit(int)"/>
    /// to immediately terminate the host process when Ctrl+C is pressed
    /// while the engine is idle.
    ///
    /// This behavior is intended for standalone demo applications
    /// and may be undesirable when the engine is embedded
    /// into a larger host.
    /// </remarks>
    public bool ExitOnCancelWhenIdle { get; init; } = true;
}
