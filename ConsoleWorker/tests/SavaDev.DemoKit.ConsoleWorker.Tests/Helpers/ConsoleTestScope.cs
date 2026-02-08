namespace SavaDev.DemoKit.ConsoleWorker.Tests.Helpers;

/// <summary>
/// Temporarily redirects console output for tests.
/// </summary>
internal sealed class ConsoleTestScope : IDisposable
{
    private readonly TextWriter _originalOut;
    private readonly TextWriter _originalError;
    private readonly StringWriter _buffer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleTestScope"/> class.
    /// </summary>
    public ConsoleTestScope()
    {
        _originalOut = Console.Out;
        _originalError = Console.Error;
        _buffer = new StringWriter();

        Console.SetOut(TextWriter.Synchronized(_buffer));
        Console.SetError(TextWriter.Synchronized(_buffer));
    }

    /// <summary>
    /// Gets the captured console output.
    /// </summary>
    public string Output => _buffer.ToString();

    /// <inheritdoc />
    public void Dispose()
    {
        Console.SetOut(_originalOut);
        Console.SetError(_originalError);
        _buffer.Dispose();
    }
}
