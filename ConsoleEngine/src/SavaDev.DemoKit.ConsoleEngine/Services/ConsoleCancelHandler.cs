namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Manages Ctrl+C handling for the demo engine.
/// </summary>
internal sealed class ConsoleCancelHandler
{
    private readonly ConsoleDemoOptions _options;
    private readonly Func<CancellationTokenSource?> _currentScenarioCtsProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleCancelHandler"/> class.
    /// </summary>
    /// <param name="options">Demo engine options.</param>
    /// <param name="currentScenarioCtsProvider">Provider for the current scenario CTS.</param>
    public ConsoleCancelHandler(
        ConsoleDemoOptions options,
        Func<CancellationTokenSource?> currentScenarioCtsProvider)
    {
        _options = options;
        _currentScenarioCtsProvider = currentScenarioCtsProvider;
    }

    /// <summary>
    /// Registers a Ctrl+C handler if enabled.
    /// </summary>
    /// <returns>The registered handler, or <c>null</c> when disabled.</returns>
    public ConsoleCancelEventHandler? Register()
    {
        if (!_options.HandleCancelKeyPress)
        {
            return null;
        }

        ConsoleCancelEventHandler handler = (_, e) =>
        {
            e.Cancel = true;

            var cts = _currentScenarioCtsProvider();
            if (cts is not null)
            {
                cts.Cancel();
            }
            else if (_options.ExitOnCancelWhenIdle)
            {
                Environment.Exit(0);
            }
        };

        Console.CancelKeyPress += handler;
        return handler;
    }
}
