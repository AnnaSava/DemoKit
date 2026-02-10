namespace SavaDev.DemoKit.ConsoleWorker.Args;

/// <summary>
/// Demo worker start options.
/// </summary>
internal sealed class ArgsOptions
{
    /// <summary>
    /// Worker mode.
    /// </summary>
    public WorkerMode Mode { get; init; } = WorkerMode.Run;

    /// <summary>
    /// Message interval in milliseconds.
    /// </summary>
    public int Interval { get; init; } = 1000;

    /// <summary>
    /// Exit delay in seconds.
    /// </summary>
    public int ExitAfterSeconds { get; init; } = 5;

    /// <summary>
    /// Console foreground color for worker output.
    /// </summary>
    public ConsoleColor Color { get; init; } = ConsoleColor.Gray;

}
