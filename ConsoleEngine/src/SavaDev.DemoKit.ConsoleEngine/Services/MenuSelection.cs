namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Represents a resolved menu selection.
/// </summary>
internal readonly struct MenuSelection
{
    /// <summary>
    /// Creates an invalid menu selection.
    /// </summary>
    /// <param name="rawInput">Raw input captured from the user.</param>
    /// <returns>An invalid selection.</returns>
    public static MenuSelection Invalid(string? rawInput)
        => new(false, false, new List<IConsoleDemoScenario>(), rawInput);

    /// <summary>
    /// Creates a selection that requests exit.
    /// </summary>
    /// <param name="rawInput">Raw input captured from the user.</param>
    /// <returns>An exit selection.</returns>
    public static MenuSelection Exit(string? rawInput)
        => new(true, true, new List<IConsoleDemoScenario>(), rawInput);

    /// <summary>
    /// Creates a valid menu selection.
    /// </summary>
    /// <param name="selectedScenarios">Selected scenarios.</param>
    /// <param name="exitRequested">Whether exit was requested.</param>
    /// <param name="rawInput">Raw input captured from the user.</param>
    /// <returns>A valid selection.</returns>
    public static MenuSelection Valid(
        List<IConsoleDemoScenario> selectedScenarios,
        bool exitRequested,
        string? rawInput)
        => new(true, exitRequested, selectedScenarios, rawInput);

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuSelection"/> struct.
    /// </summary>
    private MenuSelection(
        bool isValid,
        bool exitRequested,
        List<IConsoleDemoScenario> selectedScenarios,
        string? rawInput)
    {
        IsValid = isValid;
        ExitRequested = exitRequested;
        SelectedScenarios = selectedScenarios;
        RawInput = rawInput;
    }

    /// <summary>
    /// Gets a value indicating whether the selection is valid.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets a value indicating whether exit was requested.
    /// </summary>
    public bool ExitRequested { get; }

    /// <summary>
    /// Gets the scenarios selected by the user.
    /// </summary>
    public List<IConsoleDemoScenario> SelectedScenarios { get; }

    /// <summary>
    /// Gets the raw input that produced this selection.
    /// </summary>
    public string? RawInput { get; }
}
