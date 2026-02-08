namespace SavaDev.DemoKit.ConsoleWorker;

/// <summary>
/// Defines configuration for the demo worker process.
/// </summary>
public sealed class ConsoleWorkerOptions
{
    /// <summary>
    /// Gets the display name of the worker.
    /// </summary>
    public string WorkerName { get; init; } = "Console Demo Worker";

    /// <summary>
    /// Gets the message template for run mode heartbeats.
    /// </summary>
    /// <remarks>
    /// The template is formatted with the heartbeat index.
    /// </remarks>
    public string RunMessageTemplate { get; init; } = "Heartbeat {0}";

    /// <summary>
    /// Gets the message template for spam mode output.
    /// </summary>
    /// <remarks>
    /// The template is formatted with the spam message index.
    /// </remarks>
    public string SpamMessageTemplate { get; init; } = "Spam message #{0}";
}
