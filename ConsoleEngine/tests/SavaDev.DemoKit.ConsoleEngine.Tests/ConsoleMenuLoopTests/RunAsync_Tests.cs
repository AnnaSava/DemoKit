using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsoleMenuLoopTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleMenuLoop.RunAsync(IReadOnlyList{IConsoleDemoScenario}, CancellationToken, CancellationToken)"/>.
/// </summary>
/// <remarks>
/// These tests focus on menu selection handling
/// and scenario execution flow.
/// </remarks>
[Collection("Console")]
public sealed class RunAsync_Tests
{
    /// <summary>
    /// Verifies that entering the quit key exits
    /// the menu loop without running any scenarios.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>No scenario is executed.</item>
    /// <item>The menu loop exits gracefully.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WithQuitInput_ShouldExitWithoutRunningScenarios()
    {
        // Arrange
        var counter = new ScenarioCounter();
        var scenarios = new IConsoleDemoScenario[] { new CountingScenario("Test", counter) };
        var options = CreateTestOptions();

        // Act
        await RunMenuLoopAsync(
            scenarios,
            input: $"{options.QuitKey}\n",
            options);

        // Assert
        Assert.Equal(0, counter.Count);
    }

    /// <summary>
    /// Verifies that selecting a scenario by number
    /// executes that scenario.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The selected scenario is executed once.</item>
    /// <item>The menu loop continues after execution.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WithNumberSelection_ShouldRunScenario()
    {
        // Arrange
        var counter = new ScenarioCounter();
        var scenarios = new IConsoleDemoScenario[] { new CountingScenario("Test", counter) };
        var options = CreateTestOptions();

        // Act
        await RunMenuLoopAsync(
            scenarios,
            input: "1\nQ\n",
            options);

        // Assert
        Assert.Equal(1, counter.Count);
    }

    /// <summary>
    /// Verifies that selecting the run-all key
    /// executes all scenarios.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Every scenario is executed once.</item>
    /// <item>Execution occurs before the next menu prompt.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WithRunAllInput_ShouldRunAllScenarios()
    {
        // Arrange
        var counter = new ScenarioCounter();
        var scenarios = new IConsoleDemoScenario[]
        {
            new CountingScenario("One", counter),
            new CountingScenario("Two", counter)
        };
        var options = CreateTestOptions();

        // Act
        await RunMenuLoopAsync(
            scenarios,
            input: "A\nQ\n",
            options);

        // Assert
        Assert.Equal(2, counter.Count);
    }

    /// <summary>
    /// Verifies that invalid input displays the
    /// configured error message.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The invalid selection message is written to the console.</item>
    /// <item>The menu loop continues after the message.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WithInvalidInput_ShouldPrintInvalidSelectionMessage()
    {
        // Arrange
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("Test") };
        var options = CreateTestOptions();

        // Act
        var output = await RunMenuLoopAsync(
            scenarios,
            input: "X\nQ\n",
            options);

        // Assert
        Assert.Contains(options.InvalidSelectionMessage, output);
    }

    /// <summary>
    /// Verifies that selecting the help action
    /// invokes the configured menu action.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The help action is invoked.</item>
    /// <item>The menu continues after the action.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WithHelpAction_ShouldInvokeHelp()
    {
        // Arrange
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("Test") };
        var options = CreateTestOptions();
        var actions = new List<MenuAction>();
        var helpCalls = 0;
        actions.Add(new MenuAction("H", "Help", () => helpCalls++));

        // Act
        var output = await CaptureOutputAsync(async () =>
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await RunMenuLoopAsync(
                    scenarios,
                    input: "H\nQ\n",
                    options,
                    actions,
                    captureOutput: false)));

        // Assert
        Assert.Equal(1, helpCalls);
        Assert.Contains(options.PausePrompt, output);
    }

    /// <summary>
    /// Verifies that selecting the version action
    /// invokes the configured menu action.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The version action is invoked.</item>
    /// <item>The menu continues after the action.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WithVersionAction_ShouldInvokeVersion()
    {
        // Arrange
        var scenarios = new IConsoleDemoScenario[] { new NoOpScenario("Test") };
        var options = CreateTestOptions();
        var actions = new List<MenuAction>();
        var versionCalls = 0;
        actions.Add(new MenuAction("V", "Version", () => versionCalls++));

        // Act
        var output = await CaptureOutputAsync(async () =>
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await RunMenuLoopAsync(
                    scenarios,
                    input: "V\nQ\n",
                    options,
                    actions,
                    captureOutput: false)));

        // Assert
        Assert.Equal(1, versionCalls);
        Assert.Contains(options.PausePrompt, output);
    }

    /// <summary>
    /// Runs the menu loop with redirected input and output.
    /// </summary>
    /// <param name="scenarios">The scenarios to provide to the loop.</param>
    /// <param name="input">The input text to feed to the menu loop.</param>
    /// <param name="options">Optional menu configuration for the test.</param>
    /// <returns>The captured console output.</returns>
    private static async Task<string> RunMenuLoopAsync(
        IReadOnlyList<IConsoleDemoScenario> scenarios,
        string input,
        ConsoleDemoOptions? options = null,
        IReadOnlyList<MenuAction>? menuActions = null,
        bool captureOutput = true)
    {
        var originalOut = Console.Out;
        var originalIn = Console.In;

        using var buffer = new StringWriter();
        using var reader = new StringReader(input);

        if (captureOutput)
        {
            Console.SetOut(TextWriter.Synchronized(buffer));
        }
        Console.SetIn(reader);

        try
        {
            var menuOptions = options ?? CreateTestOptions();
            var renderer = new ConsoleMenuRenderer(menuOptions);
            var inputReader = new ConsoleMenuInputReader(menuOptions);
            var pause = new ConsolePause(menuOptions);
            var scenarioRenderer = new ConsoleScenarioRenderer(menuOptions);
            var scenarioRunner = new ScenarioRunner(scenarioRenderer, static _ => { });
            var menuLoop = new ConsoleMenuLoop(menuOptions, renderer, inputReader, scenarioRunner, pause);
            var actions = menuActions ?? new List<MenuAction>();

            await menuLoop.RunAsync(
                scenarios,
                actions,
                demoToken: CancellationToken.None,
                ct: CancellationToken.None);
        }
        finally
        {
            if (captureOutput)
            {
                Console.SetOut(originalOut);
            }
            Console.SetIn(originalIn);
        }

        return buffer.ToString();
    }

    private static ConsoleDemoOptions CreateTestOptions()
    {
        return new ConsoleDemoOptions
        {
            ClearScreenOnHeader = false,
            PauseAfterScenarios = false,
            HandleCancelKeyPress = false,
            MenuPrompt = "> ",
            QuitKey = "Q",
            RunAllKey = "A",
            PausePrompt = "Press any key to continue..."
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

    private static async Task<string> CaptureOutputAsync(Func<Task> action)
    {
        var originalOut = Console.Out;

        using var buffer = new StringWriter();
        Console.SetOut(TextWriter.Synchronized(buffer));

        try
        {
            await action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        return buffer.ToString();
    }

    private sealed class ScenarioCounter
    {
        private int _count;

        public int Count => _count;

        public void Increment() => Interlocked.Increment(ref _count);
    }

    private sealed class CountingScenario(string name, ScenarioCounter counter) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct)
        {
            counter.Increment();
            return Task.CompletedTask;
        }
    }

    private sealed class NoOpScenario(string name) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
