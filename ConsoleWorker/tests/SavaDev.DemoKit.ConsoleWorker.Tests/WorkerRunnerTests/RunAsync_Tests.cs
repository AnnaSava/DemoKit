using SavaDev.DemoKit.ConsoleWorker.Args;
using SavaDev.DemoKit.ConsoleWorker.Executor;
using SavaDev.DemoKit.ConsoleWorker.Helpers;
using SavaDev.DemoKit.ConsoleWorker.Tests.Helpers;

namespace SavaDev.DemoKit.ConsoleWorker.Tests.WorkerRunnerTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="WorkerRunner.RunAsync"/>.
/// </summary>
/// <remarks>
/// These tests focus on startup output and
/// execution flow behavior.
/// </remarks>
[Collection("Console")]
public sealed class RunAsync_Tests
{
    /// <summary>
    /// Verifies that parsed mode is printed in startup output.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The executor is invoked once.</item>
    /// <item>The parsed mode is printed in the startup output.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WhenModeExit_ShouldPrintParsedMode()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ConsoleWorkerOptions
        {
            WorkerName = "Test Worker"
        };

        var argsOptions = new ArgsOptions
        {
            Mode = WorkerMode.Exit,
            ExitAfterSeconds = 1,
            Interval = 123
        };

        var output = new ConsoleOutputHelper(argsOptions);
        var runner = new WorkerRunner(
            options,
            argsOptions,
            output: output,
            executor: new TestExecutor());

        // Act
        await runner.RunAsync();

        // Assert
        Assert.Contains("Mode: Exit", console.Output);
    }

    private sealed class TestExecutor : IWorkerModeExecutor
    {
        public Task<int> ExecuteAsync(CancellationToken ct)
        {
            return Task.FromResult(0);
        }
    }
}
