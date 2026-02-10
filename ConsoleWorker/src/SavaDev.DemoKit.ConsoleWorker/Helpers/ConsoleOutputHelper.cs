using SavaDev.DemoKit.ConsoleWorker.Args;

namespace SavaDev.DemoKit.ConsoleWorker.Helpers;

/// <summary>
/// Provides common console output helpers for worker modes.
/// </summary>
internal sealed class ConsoleOutputHelper
{
    private readonly ArgsOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleOutputHelper"/> class.
    /// </summary>
    /// <param name="options">Start options.</param>
    public ConsoleOutputHelper(ArgsOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Prints a standard hint for exiting long-running modes.
    /// </summary>
    public void PrintExitHint()
    {
        WriteLine("Press Ctrl+C to exit.");
        WriteLine();
    }

    /// <summary>
    /// Writes a line to the console using the configured output color.
    /// </summary>
    /// <param name="message">Line to write.</param>
    public void WriteLine(string message)
    {
        var originalColor = Console.ForegroundColor;
        var colorChanged = _options.Color != originalColor;
        if (colorChanged)
        {
            Console.ForegroundColor = _options.Color;
        }

        try
        {
            Console.WriteLine(message);
        }
        finally
        {
            if (colorChanged)
            {
                Console.ForegroundColor = originalColor;
            }
        }
    }

    /// <summary>
    /// Writes an empty line using the configured output color.
    /// </summary>
    public void WriteLine()
    {
        WriteLine(string.Empty);
    }
}
