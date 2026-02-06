using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsoleMenuInputReaderTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleMenuInputReader.Read(IReadOnlyList{IConsoleDemoScenario})"/>.
/// </summary>
/// <remarks>
/// These tests focus on parsing input into
/// menu selections and exit requests.
/// </remarks>
[Collection("Console")]
public sealed class Read_Tests
{
    /// <summary>
    /// Verifies that end-of-input is treated
    /// as an exit request.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is valid.</item>
    /// <item>Exit is requested.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Read_WhenInputIsNull_ShouldRequestExit()
    {
        // Arrange
        var options = CreateOptions();
        var reader = new ConsoleMenuInputReader(options);
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("One") };

        // Act
        var result = ReadFromInput(reader, scenarios, input: "");

        // Assert
        AssertSelection(
            result,
            isValid: true,
            exitRequested: true,
            expectedCount: 0);
    }

    /// <summary>
    /// Verifies that whitespace input
    /// is treated as invalid.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is invalid.</item>
    /// <item>No exit is requested.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Read_WhenInputIsWhitespace_ShouldBeInvalid()
    {
        // Arrange
        var options = CreateOptions();
        var reader = new ConsoleMenuInputReader(options);
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("One") };

        // Act
        var result = ReadFromInput(reader, scenarios, input: "   \n");

        // Assert
        AssertSelection(
            result,
            isValid: false,
            exitRequested: false,
            expectedCount: 0);
    }

    /// <summary>
    /// Verifies that the quit key
    /// requests exit.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is valid.</item>
    /// <item>Exit is requested.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Read_WhenQuitKeyProvided_ShouldRequestExit()
    {
        // Arrange
        var options = CreateOptions();
        var reader = new ConsoleMenuInputReader(options);
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("One") };

        // Act
        var result = ReadFromInput(reader, scenarios, input: $"{options.QuitKey}\n");

        // Assert
        AssertSelection(
            result,
            isValid: true,
            exitRequested: true,
            expectedCount: 0);
    }

    /// <summary>
    /// Verifies that the run-all key
    /// selects all scenarios.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is valid.</item>
    /// <item>All scenarios are selected.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Read_WhenRunAllKeyProvided_ShouldSelectAllScenarios()
    {
        // Arrange
        var options = CreateOptions();
        var reader = new ConsoleMenuInputReader(options);
        var scenarios = new IConsoleDemoScenario[]
        {
            new NoOpScenario("One"),
            new NoOpScenario("Two")
        };

        // Act
        var result = ReadFromInput(reader, scenarios, input: $"{options.RunAllKey}\n");

        // Assert
        AssertSelection(
            result,
            isValid: true,
            exitRequested: false,
            expectedCount: 2);
    }

    /// <summary>
    /// Verifies that selecting a valid index
    /// returns the matching scenario.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is valid.</item>
    /// <item>The selected scenario is returned.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Read_WhenValidIndexProvided_ShouldSelectScenario()
    {
        // Arrange
        var options = CreateOptions();
        var reader = new ConsoleMenuInputReader(options);
        var scenarios = new IConsoleDemoScenario[]
        {
            new NoOpScenario("One"),
            new NoOpScenario("Two")
        };

        // Act
        var result = ReadFromInput(reader, scenarios, input: "2\n");

        // Assert
        AssertSelection(
            result,
            isValid: true,
            exitRequested: false,
            expectedCount: 1);
        Assert.Equal("Two", result.SelectedScenarios[0].Name);
    }

    /// <summary>
    /// Verifies that out-of-range indices
    /// are treated as invalid.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is invalid.</item>
    /// <item>No scenarios are selected.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Read_WhenIndexOutOfRange_ShouldBeInvalid()
    {
        // Arrange
        var options = CreateOptions();
        var reader = new ConsoleMenuInputReader(options);
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("One") };

        // Act
        var result = ReadFromInput(reader, scenarios, input: "2\n");

        // Assert
        AssertSelection(
            result,
            isValid: false,
            exitRequested: false,
            expectedCount: 0);
    }

    /// <summary>
    /// Verifies that non-numeric input
    /// is treated as invalid.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selection is invalid.</item>
    /// <item>No scenarios are selected.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Read_WhenInputIsNotNumeric_ShouldBeInvalid()
    {
        // Arrange
        var options = CreateOptions();
        var reader = new ConsoleMenuInputReader(options);
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("One") };

        // Act
        var result = ReadFromInput(reader, scenarios, input: "foo\n");

        // Assert
        AssertSelection(
            result,
            isValid: false,
            exitRequested: false,
            expectedCount: 0);
    }

    private static ConsoleDemoOptions CreateOptions()
    {
        return new ConsoleDemoOptions
        {
            MenuPrompt = "> ",
            QuitKey = "Q",
            RunAllKey = "A"
        };
    }

    private static MenuSelection ReadFromInput(
        ConsoleMenuInputReader reader,
        IReadOnlyList<IConsoleDemoScenario> scenarios,
        string input)
    {
        var originalIn = Console.In;

        using var inputReader = new StringReader(input);
        Console.SetIn(inputReader);

        try
        {
            return reader.Read(scenarios);
        }
        finally
        {
            Console.SetIn(originalIn);
        }
    }

    private static void AssertSelection(
        MenuSelection result,
        bool isValid,
        bool exitRequested,
        int expectedCount)
    {
        Assert.Equal(isValid, result.IsValid);
        Assert.Equal(exitRequested, result.ExitRequested);
        Assert.Equal(expectedCount, result.SelectedScenarios.Count);
    }

    private sealed class NoOpScenario(string name) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
