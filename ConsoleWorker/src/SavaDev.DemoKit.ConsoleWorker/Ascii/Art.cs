namespace SavaDev.DemoKit.ConsoleWorker.Ascii;

/// <summary>
/// Provides a small animated ASCII art banner used by demo scenarios.
/// </summary>
/// <remarks>
/// This class renders a looping ASCII art animation line by line
/// with a fixed delay, creating a calm, continuous visual effect
/// in the console.
///
/// Decorative elements have a long and respectable history:
/// cat paw prints have been discovered even in medieval manuscripts,
/// so it would be unreasonable to expect a demo worker to avoid them.
/// In addition, the author’s cats, Marta and Simon, are known to
/// regularly assist with software development, primarily through
/// supervision and moral support.
///
/// As a result, this banner exists.
/// </remarks>
internal static class Art
{
    /// <summary>
    /// Delay in milliseconds between printing individual lines.
    /// </summary>
    /// <remarks>
    /// The delay is intentionally fixed to ensure smooth and predictable
    /// output pacing, avoiding sudden jumps or overly rapid scrolling.
    /// </remarks>
    private const int LinePrintDelay = 120;

    /// <summary>
    /// Gets the ASCII art lines that form the banner.
    /// </summary>
    /// <remarks>
    /// Each string represents a single line of the banner.
    /// The art is designed to be displayed sequentially and looped
    /// indefinitely, creating a simple animation effect.
    ///
    /// The exact visual meaning of the pattern is left open to
    /// interpretation, though feline influences are strongly suspected.
    /// </remarks>
    private static IReadOnlyList<string> Lines => [
        "....................................................................................................",
        "....................................................................................................",
        "............................:@@@@%....@@@@:.........................................................",
        "............................@@@@@@*.:@@@@@@:........................................................",
        "...........................:@@@@@@%.:@@@@@@:........................................................",
        "............................*@@@@@%.:@@@@@*.........................................................",
        ".......................*@@@*..::::....:::..*@@@%....................................................",
        ".......................@@@@@@.....@@@*....%@@@@@:...................................................",
        ".......................@@@@@@...:@@@@@*...%@@@@@:...................................................",
        "........................*@@@:..*@@@@@@@#:.*@@@*........@@@@%....@@@@................................",
        ".............................*@@@@@@@@@@@#............@@@@@@*.:@@@@@@...............................",
        "............................@@@@@@@@@@@@@@@...........@@@@@@%.:@@@@@@...............................",
        "............................@@@@@@@@@@@@@@@...........:@@@@@%.:@@@@@:...............................",
        "............................:@@@@@@@@@@@@@.......*@@@*..::::....:::..*@@@*..........................",
        ".................................................@@@@@@....:@@@*....*@@@@@..........................",
        ".................................................@@@@@@...*@@@@@@:..*@@@@@..........................",
        "..................................................*@@@:..%@@@@@@@@*.:#@@*...........................",
        ".......................................................%@@@@@@@@@@@%................................",
        "......................................................@@@@@@@@@@@@@@@*..............................",
        "......................................................@@@@@@@@@@@@@@@*..............................",
        ".......................................................*@@@@@@@@@@@@................................",
        "...................................................................................................."
        ];

    /// <summary>
    /// Continuously renders the banner to the console until cancellation is requested.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token used to stop rendering.
    /// </param>
    /// <remarks>
    /// The banner is printed line by line with a fixed delay between
    /// each line. Once all lines are printed, the animation restarts
    /// from the beginning, using the same delay between cycles to
    /// maintain a smooth visual rhythm.
    ///
    /// Rendering stops cooperatively when cancellation is requested,
    /// typically via Ctrl+C in the hosting environment.
    /// </remarks>
    public static async Task DrawOnScreen(
        CancellationToken ct)
    {
        var consoleWidth = TryGetConsoleWidth();
        var delay = TimeSpan.FromMilliseconds(LinePrintDelay);

        while (true)
        {
            foreach (var line in Lines)
            {
                ct.ThrowIfCancellationRequested();

                Console.WriteLine(TrimForConsole(line, consoleWidth));
                await Task.Delay(delay, ct);
            }

            await Task.Delay(delay, ct);
        }
    }

    /// <summary>
    /// Trims a banner line to better fit narrow console widths.
    /// </summary>
    /// <param name="line">The original banner line.</param>
    /// <param name="consoleWidth">
    /// The detected console width, or <c>null</c> if unavailable.
    /// </param>
    /// <returns>
    /// A trimmed version of the line suitable for the current console width.
    /// </returns>
    /// <remarks>
    /// When the console width is 80 characters or less, a fixed number
    /// of characters is removed from both the beginning and the end
    /// of the line. This helps preserve the central portion of the
    /// banner while avoiding horizontal overflow.
    /// </remarks>
    private static string TrimForConsole(string line, int? consoleWidth)
    {
        if (consoleWidth > 80)
        {
            return line;
        }

        const int trim = 10;

        if (line.Length <= trim * 2)
        {
            return string.Empty;
        }

        return line.Substring(trim, line.Length - trim * 2);
    }

    /// <summary>
    /// Attempts to determine the current console window width.
    /// </summary>
    /// <returns>
    /// The console window width in characters, or <c>null</c>
    /// if the width cannot be determined.
    /// </returns>
    /// <remarks>
    /// Accessing console dimensions may fail in non-interactive
    /// environments or when output is redirected. In such cases,
    /// the method safely returns <c>null</c> and rendering logic
    /// falls back to default behavior.
    /// </remarks>
    private static int? TryGetConsoleWidth()
    {
        try
        {
            return Console.WindowWidth;
        }
        catch
        {
            return null;
        }
    }
}
