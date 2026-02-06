namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Reads and interprets menu input for scenario selection.
/// </summary>
internal sealed class ConsoleMenuInputReader
{
    private readonly ConsoleDemoOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenuInputReader"/> class.
    /// </summary>
    /// <param name="options">Demo engine options.</param>
    public ConsoleMenuInputReader(ConsoleDemoOptions options)
    {
        _options = options;
    }

    /// <summary>
    /// Reads menu input and resolves a selection.
    /// </summary>
    /// <param name="scenarios">Available scenarios.</param>
    /// <returns>The resolved menu selection.</returns>
    public MenuSelection Read(IReadOnlyList<IConsoleDemoScenario> scenarios)
    {
        Console.Write(_options.MenuPrompt);
        var input = Console.ReadLine();

        if (input is null)
        {
            return MenuSelection.Exit(null);
        }

        return TryResolveSelection(input, scenarios);
    }

    /// <summary>
    /// Resolves the provided input into a menu selection.
    /// </summary>
    /// <param name="input">Raw input value.</param>
    /// <param name="scenarios">Available scenarios.</param>
    /// <returns>The resolved menu selection.</returns>
    private MenuSelection TryResolveSelection(
        string? input,
        IReadOnlyList<IConsoleDemoScenario> scenarios)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return MenuSelection.Invalid(input);
        }

        input = input.Trim();

        if (input.Equals(_options.QuitKey, StringComparison.OrdinalIgnoreCase))
        {
            return MenuSelection.Exit(input);
        }

        if (input.Equals(_options.RunAllKey, StringComparison.OrdinalIgnoreCase))
        {
            return MenuSelection.Valid(scenarios.ToList(), false, input);
        }

        if (!int.TryParse(input, out var index))
        {
            return MenuSelection.Invalid(input);
        }

        if (index < 1 || index > scenarios.Count)
        {
            return MenuSelection.Invalid(input);
        }

        return MenuSelection.Valid(
            new List<IConsoleDemoScenario> { scenarios[index - 1] },
            false,
            input);
    }
}
