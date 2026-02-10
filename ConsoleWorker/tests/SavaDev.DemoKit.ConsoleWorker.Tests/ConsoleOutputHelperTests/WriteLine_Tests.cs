using SavaDev.DemoKit.ConsoleWorker.Args;
using SavaDev.DemoKit.ConsoleWorker.Helpers;
using SavaDev.DemoKit.ConsoleWorker.Tests.Helpers;

namespace SavaDev.DemoKit.ConsoleWorker.Tests.ConsoleOutputHelperTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleOutputHelper.WriteLine(string)"/>.
/// </summary>
[Collection("Console")]
public sealed class WriteLine_Tests
{
    /// <summary>
    /// Verifies that output is written using the configured color
    /// and then restored to the original color.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The original color is restored after output.</item>
    /// <item>The written text is captured.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void WriteLine_WhenColorConfigured_ShouldRestoreOriginalColor()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var originalColor = Console.ForegroundColor;

        var options = new ArgsOptions
        {
            Color = ConsoleColor.Cyan
        };
        var helper = new ConsoleOutputHelper(options);

        // Act
        helper.WriteLine("Test line");

        // Assert
        Assert.Equal(originalColor, Console.ForegroundColor);
        Assert.Contains("Test line", console.Output);
    }

    /// <summary>
    /// Verifies that the empty overload writes a blank line.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An empty line is written.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void WriteLine_WhenNoParameters_ShouldWriteEmptyLine()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Color = ConsoleColor.Gray
        };
        var helper = new ConsoleOutputHelper(options);

        // Act
        helper.WriteLine();

        // Assert
        Assert.Equal(Environment.NewLine, console.Output);
    }
}
