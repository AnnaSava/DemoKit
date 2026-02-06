using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.MenuSelectionTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="MenuSelection"/> factory methods.
/// </summary>
/// <remarks>
/// These tests focus on the default values
/// returned by factory helpers.
/// </remarks>
public sealed class Factory_Tests
{
    /// <summary>
    /// Verifies that the invalid selection factory
    /// returns a non-exiting invalid selection.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is invalid.</item>
    /// <item>Exit is not requested.</item>
    /// <item>No scenarios are selected.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Invalid_ShouldReturnInvalidSelection()
    {
        // Act
        var selection = MenuSelection.Invalid(rawInput: null);

        // Assert
        Assert.False(selection.IsValid);
        Assert.False(selection.ExitRequested);
        Assert.Empty(selection.SelectedScenarios);
    }

    /// <summary>
    /// Verifies that the exit factory returns
    /// a valid exit selection.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is valid.</item>
    /// <item>Exit is requested.</item>
    /// <item>No scenarios are selected.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Exit_ShouldReturnExitSelection()
    {
        // Act
        var selection = MenuSelection.Exit(rawInput: null);

        // Assert
        Assert.True(selection.IsValid);
        Assert.True(selection.ExitRequested);
        Assert.Empty(selection.SelectedScenarios);
    }

    /// <summary>
    /// Verifies that the valid selection factory
    /// preserves selected scenarios and exit flag.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is valid.</item>
    /// <item>The exit flag is preserved.</item>
    /// <item>The scenarios are preserved.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Valid_ShouldReturnSelectionWithProvidedScenarios()
    {
        // Arrange
        var scenarios = new List<IConsoleDemoScenario>
        {
            new NoOpScenario("One"),
            new NoOpScenario("Two")
        };

        // Act
        var selection = MenuSelection.Valid(scenarios, exitRequested: false, rawInput: "1");

        // Assert
        Assert.True(selection.IsValid);
        Assert.False(selection.ExitRequested);
        Assert.Same(scenarios, selection.SelectedScenarios);
    }

    private sealed class NoOpScenario(string name) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
