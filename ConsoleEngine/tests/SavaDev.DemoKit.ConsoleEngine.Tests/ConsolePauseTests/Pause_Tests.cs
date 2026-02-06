using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsolePauseTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsolePause.Pause"/>.
/// </summary>
/// <remarks>
/// These tests focus on prompt rendering and
/// input handling behavior.
/// </remarks>
[Collection("Console")]
public sealed class Pause_Tests
{
    /// <summary>
    /// Verifies that the pause prompt is written
    /// before awaiting key input.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The pause prompt is printed.</item>
    /// <item>An exception is raised when input is redirected.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Pause_WhenInputRedirected_ShouldPrintPromptAndThrow()
    {
        // Arrange
        var options = new ConsoleDemoOptions
        {
            PausePrompt = "Press any key to continue..."
        };

        var pause = new ConsolePause(options);
        var originalIn = Console.In;

        using var reader = new StringReader("x");
        Console.SetIn(reader);

        try
        {
            // Act
            var output = CaptureOutput(() =>
                Assert.Throws<InvalidOperationException>(() => pause.Pause()));

            // Assert
            Assert.Contains(options.PausePrompt, output);
        }
        finally
        {
            Console.SetIn(originalIn);
        }
    }

    private static string CaptureOutput(Action action)
    {
        var originalOut = Console.Out;

        using var buffer = new StringWriter();
        Console.SetOut(TextWriter.Synchronized(buffer));

        try
        {
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        return buffer.ToString();
    }
}
