namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Renders the demo menu and related messages.
/// </summary>
internal sealed class ConsoleMenuRenderer
{
    private readonly ConsoleDemoOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenuRenderer"/> class.
    /// </summary>
    /// <param name="options">Demo engine options.</param>
    public ConsoleMenuRenderer(ConsoleDemoOptions options)
    {
        _options = options;
    }

    /// <summary>
    /// Prints the menu header.
    /// </summary>
    public void PrintHeader()
    {
        if (_options.ClearScreenOnHeader)
        {
            TryClearConsole();
        }

        Console.WriteLine(_options.HeaderBorderLine);
        Console.WriteLine($" {_options.Title} ");
        Console.WriteLine(_options.HeaderBorderLine);
        Console.WriteLine();
    }

    /// <summary>
    /// Prints the menu options and scenario list.
    /// </summary>
    /// <param name="scenarios">Available scenarios.</param>
    /// <param name="menuActions">Additional menu actions.</param>
    public void PrintMenu(
        IReadOnlyList<IConsoleDemoScenario> scenarios,
        IReadOnlyList<MenuAction> menuActions)
    {
        for (var i = 0; i < scenarios.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {scenarios[i].Name}");
        }

        Console.WriteLine();
        Console.WriteLine($"{_options.RunAllKey}. {_options.RunAllLabel}");
        PrintMenuActions(menuActions);
        Console.WriteLine($"{_options.QuitKey}. {_options.QuitLabel}");
        Console.WriteLine();
    }

    /// <summary>
    /// Prints the invalid selection message.
    /// </summary>
    public void PrintInvalidSelection()
    {
        Console.WriteLine(_options.InvalidSelectionMessage);
    }

    /// <summary>
    /// Clears the console screen when possible.
    /// </summary>
    private static void TryClearConsole()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Console output may be redirected or unavailable.
            // Clearing the screen is optional and should not
            // interrupt demo execution.
        }
    }

    /// <summary>
    /// Prints additional menu actions.
    /// </summary>
    /// <param name="menuActions">Actions to display.</param>
    private static void PrintMenuActions(IReadOnlyList<MenuAction> menuActions)
    {
        foreach (var action in menuActions)
        {
            Console.WriteLine($"{action.Key}. {action.Label}");
        }
    }
}
