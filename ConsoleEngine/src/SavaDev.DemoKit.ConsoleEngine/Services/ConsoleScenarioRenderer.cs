namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Renders scenario execution output.
/// </summary>
internal sealed class ConsoleScenarioRenderer
{
    private readonly ConsoleDemoOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleScenarioRenderer"/> class.
    /// </summary>
    /// <param name="options">Demo engine options.</param>
    public ConsoleScenarioRenderer(ConsoleDemoOptions options)
    {
        _options = options;
    }

    /// <summary>
    /// Prints the header for a running scenario.
    /// </summary>
    /// <param name="scenarioName">Scenario display name.</param>
    public void PrintScenarioHeader(string scenarioName)
    {
        Console.WriteLine();
        Console.WriteLine(_options.SeparatorLine);
        Console.WriteLine(string.Format(_options.ScenarioHeaderFormat, scenarioName));
        Console.WriteLine(_options.SeparatorLine);
        Console.WriteLine();
    }

    /// <summary>
    /// Prints the footer after a scenario completes.
    /// </summary>
    public void PrintScenarioFooter()
    {
        Console.WriteLine();
        Console.WriteLine(_options.SeparatorLine);
        Console.WriteLine(_options.ScenarioFooterTitle);
        Console.WriteLine(_options.SeparatorLine);
        Console.WriteLine();
    }

    /// <summary>
    /// Prints the cancellation message for a scenario.
    /// </summary>
    public void PrintCancelled()
    {
        Console.WriteLine();
        Console.WriteLine(_options.ScenarioCancelledMessage);
    }

    /// <summary>
    /// Prints the unexpected error header and exception.
    /// </summary>
    /// <param name="ex">The exception that occurred.</param>
    public void PrintUnexpectedError(Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine(_options.UnexpectedErrorHeader);
        Console.WriteLine(ex);
    }
}
