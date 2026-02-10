using SavaDev.DemoKit.ConsoleWorker.Args;
using SavaDev.DemoKit.ConsoleWorker.Executor;
using SavaDev.DemoKit.ConsoleWorker.Helpers;
using SavaDev.DemoKit.ConsoleWorker.Tests.Helpers;

namespace SavaDev.DemoKit.ConsoleWorker.Tests.WorkerModeExecutorTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="WorkerModeExecutor.ExecuteAsync"/>.
/// </summary>
/// <remarks>
/// These tests focus on mode-specific execution,
/// output, and cancellation behavior.
/// </remarks>
[Collection("Console")]
public sealed class ExecuteAsync_Tests
{
    /// <summary>
    /// Verifies that cancelling run mode
    /// propagates <see cref="OperationCanceledException"/>.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="OperationCanceledException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeRunCancelled_ShouldThrow()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Run,
            Interval = 1
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));
    }

    /// <summary>
    /// Verifies that run mode prints heartbeat output.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>A heartbeat line is printed.</item>
    /// <item>The worker can be cancelled.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeRun_ShouldPrintHeartbeat()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Run,
            Interval = 1
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));

        // Assert
        Assert.Contains("Heartbeat", console.Output);
    }

    /// <summary>
    /// Verifies that run mode prints the Ctrl+C hint.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The exit hint is printed.</item>
    /// <item>The worker can be cancelled.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeRun_ShouldPrintExitHint()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Run,
            Interval = 1
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));

        // Assert
        Assert.Contains("Press Ctrl+C to exit.", console.Output);
    }

    /// <summary>
    /// Verifies that exit mode returns zero
    /// after the configured delay.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The exit code is zero.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeExit_ShouldReturnZero()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Exit,
            ExitAfterSeconds = 0
        };
        var executor = CreateExecutor(options);

        // Act
        var exitCode = await executor.ExecuteAsync(CancellationToken.None);

        // Assert
        Assert.Equal(0, exitCode);
    }

    /// <summary>
    /// Verifies that crash mode throws
    /// an <see cref="InvalidOperationException"/>.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="InvalidOperationException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeCrash_ShouldThrow()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Crash
        };
        var executor = CreateExecutor(options);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => executor.ExecuteAsync(CancellationToken.None));

        // Assert
        Assert.Contains("Intentional crash", exception.Message);
    }

    /// <summary>
    /// Verifies that cancelling spam mode
    /// propagates <see cref="OperationCanceledException"/>.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="OperationCanceledException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeSpamCancelled_ShouldThrow()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Spam,
            Interval = 1
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));
    }

    /// <summary>
    /// Verifies that spam mode prints spam output.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>A spam message is printed.</item>
    /// <item>The worker can be cancelled.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeSpam_ShouldPrintSpamMessage()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Spam,
            Interval = 1
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));

        // Assert
        Assert.Contains("Spam message", console.Output);
    }

    /// <summary>
    /// Verifies that spam mode prints the Ctrl+C hint.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The exit hint is printed.</item>
    /// <item>The worker can be cancelled.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeSpam_ShouldPrintExitHint()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Spam,
            Interval = 1
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));

        // Assert
        Assert.Contains("Press Ctrl+C to exit.", console.Output);
    }

    /// <summary>
    /// Verifies that cancelling echo mode
    /// propagates <see cref="OperationCanceledException"/>.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="OperationCanceledException"/> is thrown.</item>
    /// <item>The echo prompt is printed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeEchoCancelled_ShouldThrow()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Echo
        };
        var executor = CreateExecutor(options);

        var originalIn = Console.In;
        using var reader = new StringReader(string.Empty);
        Console.SetIn(reader);

        try
        {
            using var cts = CreateTestCancellation();

            // Act
            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => executor.ExecuteAsync(cts.Token));

            // Assert
            Assert.Contains("Echo mode", console.Output);
        }
        finally
        {
            Console.SetIn(originalIn);
        }
    }

    /// <summary>
    /// Verifies that echo mode prints input back to the user.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The echo prompt is printed.</item>
    /// <item>The input line is echoed.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeEcho_ShouldEchoInput()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Echo
        };
        var executor = CreateExecutor(options);

        var originalIn = Console.In;
        using var reader = new StringReader("hello");
        Console.SetIn(reader);

        try
        {
            using var cts = CreateTestCancellation();

            // Act
            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => executor.ExecuteAsync(cts.Token));

            // Assert
            Assert.Contains("Echo mode", console.Output);
            Assert.Contains("hello", console.Output);
        }
        finally
        {
            Console.SetIn(originalIn);
        }
    }

    /// <summary>
    /// Verifies that art mode prints the Ctrl+C hint.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The exit hint is printed.</item>
    /// <item>The worker can be cancelled.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeArt_ShouldPrintExitHint()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Art
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));

        // Assert
        Assert.Contains("Press Ctrl+C to exit.", console.Output);
    }

    /// <summary>
    /// Verifies that art mode prints banner output.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>At least one banner line is printed.</item>
    /// <item>The worker can be cancelled.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public async Task ExecuteAsync_WhenModeArt_ShouldPrintBannerLine()
    {
        // Arrange
        using var console = new ConsoleTestScope();
        var options = new ArgsOptions
        {
            Mode = WorkerMode.Art
        };
        var executor = CreateExecutor(options);

        using var cts = CreateTestCancellation(500);

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => executor.ExecuteAsync(cts.Token));

        // Assert
        Assert.Contains("..", console.Output);
    }

    /// <summary>
    /// Creates a worker mode executor with default worker options
    /// and a console output helper based on the provided args.
    /// </summary>
    /// <param name="options">Parsed start options.</param>
    /// <returns>Executor instance for test execution.</returns>
    private static WorkerModeExecutor CreateExecutor(ArgsOptions options)
    {
        var output = new ConsoleOutputHelper(options);
        return new WorkerModeExecutor(options, new ConsoleWorkerOptions(), output);
    }

    /// <summary>
    /// Creates a cancellation token source configured to cancel
    /// after the specified timeout.
    /// </summary>
    /// <param name="timeoutMs">Timeout in milliseconds.</param>
    /// <returns>Cancellation token source for test execution.</returns>
    private static CancellationTokenSource CreateTestCancellation(int timeoutMs = 300)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(timeoutMs);
        return cts;
    }
}
