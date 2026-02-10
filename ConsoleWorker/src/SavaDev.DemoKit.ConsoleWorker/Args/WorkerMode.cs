namespace SavaDev.DemoKit.ConsoleWorker.Args;

/// <summary>
/// Demo worker operating modes.
/// </summary>
internal enum WorkerMode
{
    /// <summary>
    /// Continuous mode with heartbeat messages.
    /// </summary>
    Run,

    /// <summary>
    /// Exit after a delay.
    /// </summary>
    Exit,

    /// <summary>
    /// Intentional crash.
    /// </summary>
    Crash,

    /// <summary>
    /// Continuous spam output.
    /// </summary>
    Spam,

    /// <summary>
    /// Echoes user input until cancellation.
    /// </summary>
    Echo,

    /// <summary>
    /// ASCII art mode.
    /// </summary>
    Art
}
