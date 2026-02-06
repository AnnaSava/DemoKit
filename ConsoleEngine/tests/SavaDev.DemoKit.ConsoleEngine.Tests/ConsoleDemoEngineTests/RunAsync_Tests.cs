namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsoleDemoEngineTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleDemoEngine.RunAsync(IReadOnlyList{IConsoleDemoScenario}, CancellationToken)"/>.
/// </summary>
/// <remarks>
/// These tests focus on menu parsing, scenario execution,
/// and user interaction flow.
/// </remarks>
[Collection("Console")]
public sealed class RunAsync_Tests
{



    /// <summary>
    /// Runs the demo engine with redirected input and output.
    /// </summary>
    /// <param name="scenarios">The scenarios to provide to the engine.</param>
    /// <param name="input">The input text to feed to the menu loop.</param>
    /// <param name="options">Optional engine configuration for the test.</param>
    /// <returns>The captured console output.</returns>
    private static async Task<string> RunEngineAsync(
        IReadOnlyList<IConsoleDemoScenario> scenarios,
        string input,
        ConsoleDemoOptions? options = null)
    {
        var originalOut = Console.Out;
        var originalIn = Console.In;

        using var buffer = new StringWriter();
        using var reader = new StringReader(input);

        Console.SetOut(TextWriter.Synchronized(buffer));
        Console.SetIn(reader);

        try
        {
            var engine = new ConsoleDemoEngine(options ?? CreateTestOptions(), Array.Empty<string>());
            await engine.RunAsync(scenarios);
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetIn(originalIn);
        }

        return buffer.ToString();
    }

    /// <summary>
    /// Creates options suitable for automated tests.
    /// </summary>
    /// <returns>Configured demo options.</returns>
    private static ConsoleDemoOptions CreateTestOptions()
    {
        return new ConsoleDemoOptions
        {
            ClearScreenOnHeader = false,
            PauseAfterScenarios = false,
            HandleCancelKeyPress = false
        };
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
