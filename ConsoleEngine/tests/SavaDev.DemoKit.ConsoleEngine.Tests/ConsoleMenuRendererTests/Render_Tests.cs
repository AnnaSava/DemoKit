using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsoleMenuRendererTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleMenuRenderer"/> output.
/// </summary>
/// <remarks>
/// These tests focus on header, menu, and
/// invalid selection rendering.
/// </remarks>
[Collection("Console")]
public sealed class Render_Tests
{
    /// <summary>
    /// Verifies that the header is rendered using
    /// the configured border and title.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The border line is printed.</item>
    /// <item>The title is printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void PrintHeader_ShouldRenderConfiguredHeader()
    {
        // Arrange
        var options = CreateTestOptions();
        var renderer = new ConsoleMenuRenderer(options);

        // Act
        var output = CaptureOutput(() => renderer.PrintHeader());

        // Assert
        Assert.Contains(options.HeaderBorderLine, output);
        Assert.Contains($" {options.Title} ", output);
    }

    /// <summary>
    /// Verifies that the menu renders scenario
    /// names with numbering and control keys.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Scenario names are printed with numbers.</item>
    /// <item>Run-all and quit labels are printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void PrintMenu_ShouldRenderScenarioNamesAndControls()
    {
        // Arrange
        var options = CreateTestOptions();
        var renderer = new ConsoleMenuRenderer(options);
        var scenarios = new IConsoleDemoScenario[]
        {
            new NoOpScenario("One"),
            new NoOpScenario("Two")
        };

        // Act
        var output = CaptureOutput(() => renderer.PrintMenu(scenarios, BuildMenuActions()));

        // Assert
        Assert.Contains("1. One", output);
        Assert.Contains("2. Two", output);
        Assert.Contains($"{options.RunAllKey}. {options.RunAllLabel}", output);
        Assert.Contains("H. Help", output);
        Assert.Contains("V. Version", output);
        Assert.Contains($"{options.QuitKey}. {options.QuitLabel}", output);
    }

    /// <summary>
    /// Verifies that invalid selection output uses
    /// the configured message.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The invalid selection message is printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void PrintInvalidSelection_ShouldRenderConfiguredMessage()
    {
        // Arrange
        var options = CreateTestOptions();
        var renderer = new ConsoleMenuRenderer(options);

        // Act
        var output = CaptureOutput(() => renderer.PrintInvalidSelection());

        // Assert
        Assert.Contains(options.InvalidSelectionMessage, output);
    }

    private static ConsoleDemoOptions CreateTestOptions()
    {
        return new ConsoleDemoOptions
        {
            ClearScreenOnHeader = false,
            PauseAfterScenarios = false,
            HandleCancelKeyPress = false,
            Title = "Menu Test",
            HeaderBorderLine = "***",
            RunAllKey = "A",
            RunAllLabel = "Run All",
            QuitKey = "Q",
            QuitLabel = "Quit",
            InvalidSelectionMessage = "Invalid!"
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

    private static IReadOnlyList<MenuAction> BuildMenuActions()
    {
        return new List<MenuAction>
        {
            new("H", "Help", static () => { }),
            new("V", "Version", static () => { })
        };
    }

    private sealed class NoOpScenario(string name) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
