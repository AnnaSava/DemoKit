namespace SavaDev.DemoKit.ConsoleWorker.Args;

/// <summary>
/// Represents the parsed worker arguments with any warnings.
/// </summary>
internal sealed class ArgsParseResult
{
    /// <summary>
    /// Gets the parsed start options.
    /// </summary>
    public required ArgsOptions Options { get; init; }

    /// <summary>
    /// Gets warnings produced during parsing.
    /// </summary>
    public IReadOnlyList<string> Warnings { get; init; } = Array.Empty<string>();
}
