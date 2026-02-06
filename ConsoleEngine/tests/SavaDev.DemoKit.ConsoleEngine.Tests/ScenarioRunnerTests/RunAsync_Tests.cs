using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ScenarioRunnerTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ScenarioRunner.RunAsync"/>.
/// </summary>
/// <remarks>
/// These tests focus on scenario execution
/// and output rendering behavior.
/// </remarks>
[Collection("Console")]
public sealed class RunAsync_Tests
{
    /// <summary>
    /// Verifies that multiple scenarios are executed
    /// in sequence.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Each scenario is executed once.</item>
    /// <item>The scenarios are executed in order.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WithMultipleScenarios_ShouldExecuteEachScenario()
    {
        // Arrange
        var renderer = CreateRenderer();
        var order = new List<string>();
        var scenarios = new IConsoleDemoScenario[]
        {
            new RecordingScenario("One", order),
            new RecordingScenario("Two", order)
        };

        var runner = new ScenarioRunner(renderer, static _ => { });

        // Act
        await runner.RunAsync(scenarios, CancellationToken.None);

        // Assert
        Assert.Equal(new[] { "One", "Two" }, order);
    }

    /// <summary>
    /// Verifies that cancelled scenarios render
    /// the cancellation message.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The cancellation message is rendered.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void RunAsync_WhenScenarioCancels_ShouldRenderCancellationMessage()
    {
        // Arrange
        var renderer = CreateRenderer();
        var scenarios = new IConsoleDemoScenario[] { new CancellingScenario("Cancel") };
        var runner = new ScenarioRunner(renderer, static _ => { });

        // Act
        var output = CaptureOutput(() => runner.RunAsync(scenarios, CancellationToken.None));

        // Assert
        Assert.Contains("Cancelled", output);
    }

    /// <summary>
    /// Verifies that scenario exceptions render
    /// the unexpected error header.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The unexpected error header is rendered.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void RunAsync_WhenScenarioThrows_ShouldRenderUnexpectedErrorHeader()
    {
        // Arrange
        var renderer = CreateRenderer();
        var scenarios = new IConsoleDemoScenario[] { new FailingScenario("Fail") };
        var runner = new ScenarioRunner(renderer, static _ => { });

        // Act
        var output = CaptureOutput(() => runner.RunAsync(scenarios, CancellationToken.None));

        // Assert
        Assert.Contains("Unexpected", output);
    }

    /// <summary>
    /// Verifies that the current scenario token source
    /// is set during execution and cleared afterwards.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The CTS is set while a scenario runs.</item>
    /// <item>The CTS is cleared after execution.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_ShouldSetAndClearCurrentScenarioCts()
    {
        // Arrange
        var renderer = CreateRenderer();
        CancellationTokenSource? current = null;
        var scenario = new CapturingScenario("Test", () => current);
        var runner = new ScenarioRunner(renderer, cts => current = cts);

        // Act
        await runner.RunAsync([scenario], CancellationToken.None);

        // Assert
        Assert.NotNull(scenario.CapturedCts);
        Assert.Null(current);
    }

    private static ConsoleScenarioRenderer CreateRenderer()
    {
        var options = new ConsoleDemoOptions
        {
            SeparatorLine = "---",
            ScenarioHeaderFormat = "Header: {0}",
            ScenarioFooterTitle = "Footer",
            ScenarioCancelledMessage = "Cancelled",
            UnexpectedErrorHeader = "Unexpected"
        };

        return new ConsoleScenarioRenderer(options);
    }

    private static string CaptureOutput(Func<Task> action)
    {
        var originalOut = Console.Out;

        using var buffer = new StringWriter();
        Console.SetOut(TextWriter.Synchronized(buffer));

        try
        {
            action().GetAwaiter().GetResult();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        return buffer.ToString();
    }

    private sealed class RecordingScenario(string name, List<string> order) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct)
        {
            order.Add(Name);
            return Task.CompletedTask;
        }
    }

    private sealed class CancellingScenario(string name) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct)
            => Task.FromCanceled(ct.IsCancellationRequested ? ct : new CancellationToken(true));
    }

    private sealed class FailingScenario(string name) : IConsoleDemoScenario
    {
        public string Name { get; } = name;

        public Task RunAsync(CancellationToken ct)
            => Task.FromException(new InvalidOperationException("boom"));
    }

    private sealed class CapturingScenario(string name, Func<CancellationTokenSource?> currentCtsProvider)
        : IConsoleDemoScenario
    {
        private readonly Func<CancellationTokenSource?> _currentCtsProvider = currentCtsProvider;

        public string Name { get; } = name;

        public CancellationTokenSource? CapturedCts { get; private set; }

        public Task RunAsync(CancellationToken ct)
        {
            CapturedCts = _currentCtsProvider();
            return Task.CompletedTask;
        }
    }
}
