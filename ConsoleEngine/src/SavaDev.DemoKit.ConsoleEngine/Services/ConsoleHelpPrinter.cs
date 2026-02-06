namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Renders help text for the demo host.
/// </summary>
internal sealed class ConsoleHelpPrinter
{
    private readonly ConsoleDemoOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleHelpPrinter"/> class.
    /// </summary>
    /// <param name="options">Demo engine options.</param>
    public ConsoleHelpPrinter(ConsoleDemoOptions options)
    {
        _options = options;
    }

    /// <summary>
    /// Writes help information to the console.
    /// </summary>
    public void Print()
    {
        var appName = AppDomain.CurrentDomain.FriendlyName;

        Console.WriteLine($"{appName} - {_options.Title}");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine($"  {appName} [--help|-h] [--version|-v]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --help, -h       Show this help and exit.");
        Console.WriteLine("  --version, -v    Show version information and exit.");
        Console.WriteLine();
    }
}
