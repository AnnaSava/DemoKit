using SavaDev.DemoKit.ConsoleWorker.Tests.Helpers;

namespace SavaDev.DemoKit.ConsoleWorker.Tests.ConsoleDemoWorkerTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleDemoWorker.RunAsync"/>.
/// </summary>
/// <remarks>
/// These tests focus on argument parsing and
/// execution flow behavior.
/// </remarks>
[Collection("Console")]
public sealed class RunAsync_Tests
{
    /// <summary>
    /// Verifies that a worker instance cannot be run twice.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The first run completes successfully.</item>
    /// <item>The second run throws <see cref="InvalidOperationException"/>.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WhenCalledTwice_ShouldThrow()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--mode", "exit", "--exit-after", "1" });

        // Act
        await worker.RunAsync();

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => worker.RunAsync());
    }

    /// <summary>
    /// Verifies that invalid arguments stop execution and return error code.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The constructor completes without throwing.</item>
    /// <item>An error message is written to output.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task RunAsync_WhenArgsInvalid_ShouldReturnErrorCode()
    {
        // Arrange
        using var console = new ConsoleTestScope();

        // Act
        var worker = new ConsoleDemoWorker(
            options: new ConsoleWorkerOptions(),
            args: new[] { "--interval" });
        var exitCode = await worker.RunAsync();

        // Assert
        Assert.Equal(1, exitCode);
        Assert.Contains("--interval", console.Output);
    }
}
