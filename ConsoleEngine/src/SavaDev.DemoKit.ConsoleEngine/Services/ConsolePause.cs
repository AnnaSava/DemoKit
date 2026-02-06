namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Pauses execution and waits for a key press.
/// </summary>
internal sealed class ConsolePause
{
    private readonly ConsoleDemoOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsolePause"/> class.
    /// </summary>
    /// <param name="options">Demo engine options.</param>
    public ConsolePause(ConsoleDemoOptions options)
    {
        _options = options;
    }

    /// <summary>
    /// Prints the pause prompt and waits for a key press.
    /// </summary>
    public void Pause()
    {
        Console.WriteLine(_options.PausePrompt);
        Console.ReadKey(intercept: true);
    }
}
