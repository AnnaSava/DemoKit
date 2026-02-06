namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Executes scenarios and renders their output.
/// </summary>
internal sealed class ScenarioRunner
{
    private readonly ConsoleScenarioRenderer _renderer;
    private readonly Action<CancellationTokenSource?> _setCurrentScenarioCts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScenarioRunner"/> class.
    /// </summary>
    /// <param name="renderer">Scenario output renderer.</param>
    /// <param name="setCurrentScenarioCts">Setter for current scenario CTS.</param>
    public ScenarioRunner(
        ConsoleScenarioRenderer renderer,
        Action<CancellationTokenSource?> setCurrentScenarioCts)
    {
        _renderer = renderer;
        _setCurrentScenarioCts = setCurrentScenarioCts;
    }

    /// <summary>
    /// Executes all provided scenarios in order.
    /// </summary>
    /// <param name="scenarios">Scenarios to execute.</param>
    /// <param name="ct">Cancellation token for execution.</param>
    public async Task RunAsync(
        IReadOnlyList<IConsoleDemoScenario> scenarios,
        CancellationToken ct)
    {
        foreach (var scenario in scenarios)
        {
            await RunScenarioAsync(scenario, ct);
        }
    }

    /// <summary>
    /// Executes a single scenario with error handling.
    /// </summary>
    /// <param name="scenario">The scenario to execute.</param>
    /// <param name="ct">Cancellation token for the scenario.</param>
    private async Task RunScenarioAsync(
        IConsoleDemoScenario scenario,
        CancellationToken ct)
    {
        using var scenarioCts =
            CancellationTokenSource.CreateLinkedTokenSource(ct);

        _setCurrentScenarioCts(scenarioCts);

        _renderer.PrintScenarioHeader(scenario.Name);

        try
        {
            await scenario.RunAsync(scenarioCts.Token);
        }
        catch (OperationCanceledException)
        {
            _renderer.PrintCancelled();
        }
        catch (Exception ex)
        {
            _renderer.PrintUnexpectedError(ex);
        }
        finally
        {
            _setCurrentScenarioCts(null);
        }

        _renderer.PrintScenarioFooter();
    }
}
