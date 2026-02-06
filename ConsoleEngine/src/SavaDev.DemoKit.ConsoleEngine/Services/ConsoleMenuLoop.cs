namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Runs the interactive menu loop for scenario selection.
/// </summary>
internal sealed class ConsoleMenuLoop
{
    private readonly ConsoleDemoOptions _options;
    private readonly ConsoleMenuRenderer _renderer;
    private readonly ConsoleMenuInputReader _inputReader;
    private readonly ScenarioRunner _scenarioRunner;
    private readonly ConsolePause _pause;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenuLoop"/> class.
    /// </summary>
    /// <param name="options">Demo engine options.</param>
    /// <param name="renderer">Menu renderer.</param>
    /// <param name="inputReader">Menu input reader.</param>
    /// <param name="scenarioRunner">Scenario runner.</param>
    /// <param name="pause">Pause helper.</param>
    public ConsoleMenuLoop(
        ConsoleDemoOptions options,
        ConsoleMenuRenderer renderer,
        ConsoleMenuInputReader inputReader,
        ScenarioRunner scenarioRunner,
        ConsolePause pause)
    {
        _options = options;
        _renderer = renderer;
        _inputReader = inputReader;
        _scenarioRunner = scenarioRunner;
        _pause = pause;
    }

    /// <summary>
    /// Runs the menu loop until exit is requested or cancellation occurs.
    /// </summary>
    /// <param name="scenarios">Available scenarios.</param>
    /// <param name="menuActions">Additional menu actions.</param>
    /// <param name="demoToken">A token linked to the demo lifetime.</param>
    /// <param name="ct">A cancellation token that can stop the loop.</param>
    public async Task RunAsync(
        IReadOnlyList<IConsoleDemoScenario> scenarios,
        IReadOnlyList<MenuAction> menuActions,
        CancellationToken demoToken,
        CancellationToken ct)
    {
        var actionMap = BuildActionMap(menuActions);

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            _renderer.PrintHeader();
            _renderer.PrintMenu(scenarios, menuActions);

            var selection = _inputReader.Read(scenarios);
            if (TryHandleMenuAction(actionMap, selection.RawInput))
            {
                continue;
            }

            if (!selection.IsValid)
            {
                _renderer.PrintInvalidSelection();
                if (_options.PauseAfterScenarios)
                {
                    _pause.Pause();
                }
                continue;
            }

            if (selection.ExitRequested)
            {
                return;
            }

            await _scenarioRunner.RunAsync(selection.SelectedScenarios, demoToken);

            if (_options.PauseAfterScenarios)
            {
                _pause.Pause();
            }
        }
    }

    /// <summary>
    /// Attempts to execute a menu action based on the raw input.
    /// </summary>
    /// <param name="menuActions">Action lookup by key.</param>
    /// <param name="rawInput">Raw user input.</param>
    /// <returns>
    /// <c>true</c> if a matching action was executed; otherwise, <c>false</c>.
    /// </returns>
    private bool TryHandleMenuAction(
        IReadOnlyDictionary<string, MenuAction> menuActions,
        string? rawInput)
    {
        if (string.IsNullOrWhiteSpace(rawInput))
        {
            return false;
        }

        var key = rawInput.Trim();
        if (!menuActions.TryGetValue(key, out var action))
        {
            return false;
        }

        action.Action();
        _pause.Pause();
        return true;
    }

    /// <summary>
    /// Builds a lookup dictionary for menu actions.
    /// </summary>
    /// <param name="menuActions">Menu actions list.</param>
    /// <returns>A case-insensitive action map.</returns>
    private static IReadOnlyDictionary<string, MenuAction> BuildActionMap(
        IReadOnlyList<MenuAction> menuActions)
    {
        var map = new Dictionary<string, MenuAction>(StringComparer.OrdinalIgnoreCase);
        foreach (var action in menuActions)
        {
            map[action.Key] = action;
        }

        return map;
    }
}
