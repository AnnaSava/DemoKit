using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsoleScenarioRendererTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleScenarioRenderer"/> output.
/// </summary>
/// <remarks>
/// These tests focus on scenario header, footer,
/// cancellation, and error rendering.
/// </remarks>
[Collection("Console")]
public sealed class Render_Tests
{
    /// <summary>
    /// Verifies that the scenario header uses
    /// the configured formatting.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The header includes the formatted scenario name.</item>
    /// <item>The separator line is printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void PrintScenarioHeader_ShouldRenderFormattedHeader()
    {
        // Arrange
        var options = CreateTestOptions();
        var renderer = new ConsoleScenarioRenderer(options);

        // Act
        var output = CaptureOutput(() => renderer.PrintScenarioHeader("HeaderTest"));

        // Assert
        Assert.Contains(">> HeaderTest <<", output);
        Assert.Contains(options.SeparatorLine, output);
    }

    /// <summary>
    /// Verifies that the scenario footer uses
    /// the configured title.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The footer title is printed.</item>
    /// <item>The separator line is printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void PrintScenarioFooter_ShouldRenderFooterTitle()
    {
        // Arrange
        var options = CreateTestOptions();
        var renderer = new ConsoleScenarioRenderer(options);

        // Act
        var output = CaptureOutput(renderer.PrintScenarioFooter);

        // Assert
        Assert.Contains(options.ScenarioFooterTitle, output);
        Assert.Contains(options.SeparatorLine, output);
    }

    /// <summary>
    /// Verifies that cancelled scenarios render
    /// the configured cancellation message.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The cancellation message is printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void PrintCancelled_ShouldRenderCancellationMessage()
    {
        // Arrange
        var options = CreateTestOptions();
        var renderer = new ConsoleScenarioRenderer(options);

        // Act
        var output = CaptureOutput(renderer.PrintCancelled);

        // Assert
        Assert.Contains(options.ScenarioCancelledMessage, output);
    }

    /// <summary>
    /// Verifies that unexpected errors render
    /// the configured header and exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The unexpected error header is printed.</item>
    /// <item>The exception details are printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void PrintUnexpectedError_ShouldRenderErrorHeaderAndException()
    {
        // Arrange
        var options = CreateTestOptions();
        var renderer = new ConsoleScenarioRenderer(options);
        var exception = new InvalidOperationException("boom");

        // Act
        var output = CaptureOutput(() => renderer.PrintUnexpectedError(exception));

        // Assert
        Assert.Contains(options.UnexpectedErrorHeader, output);
        Assert.Contains("InvalidOperationException", output);
        Assert.Contains("boom", output);
    }

    private static ConsoleDemoOptions CreateTestOptions()
    {
        return new ConsoleDemoOptions
        {
            SeparatorLine = "---",
            ScenarioHeaderFormat = ">> {0} <<",
            ScenarioFooterTitle = "=== Done ===",
            ScenarioCancelledMessage = "Cancelled!",
            UnexpectedErrorHeader = "Unexpected:"
        };
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
