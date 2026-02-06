using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsoleCancelHandlerTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleCancelHandler.Register"/>.
/// </summary>
/// <remarks>
/// These tests focus on cancellation handling and
/// registration behavior for Ctrl+C.
/// </remarks>
[Collection("Console")]
public sealed class HandleCancel_Tests
{
    /// <summary>
    /// Verifies that cancel handling is disabled
    /// when configured accordingly.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>No handler is registered.</item>
    /// <item>The result is <c>null</c>.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Register_WhenCancelHandlingDisabled_ShouldReturnNull()
    {
        // Arrange
        var options = new ConsoleDemoOptions
        {
            HandleCancelKeyPress = false
        };

        var handler = new ConsoleCancelHandler(options, static () => null);

        // Act
        var result = handler.Register();

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Verifies that a running scenario is cancelled
    /// when Ctrl+C is handled.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The cancel event is marked as handled.</item>
    /// <item>The current scenario token source is cancelled.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Register_WhenScenarioRunning_ShouldCancelScenario()
    {
        // Arrange
        var options = new ConsoleDemoOptions
        {
            HandleCancelKeyPress = true,
            ExitOnCancelWhenIdle = false
        };

        using var cts = new CancellationTokenSource();

        var handler = new ConsoleCancelHandler(options, () => cts);
        var cancelHandler = handler.Register();
        var args = CreateCancelEventArgs();

        try
        {
            // Act
            cancelHandler?.Invoke(this, args);

            // Assert
            Assert.True(args.Cancel);
            Assert.True(cts.IsCancellationRequested);
        }
        finally
        {
            if (cancelHandler is not null)
            {
                Console.CancelKeyPress -= cancelHandler;
            }
        }
    }

    /// <summary>
    /// Verifies that idle cancellation does not exit
    /// when exit behavior is disabled.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The cancel event is marked as handled.</item>
    /// <item>No exception is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Register_WhenIdleAndExitDisabled_ShouldOnlyMarkHandled()
    {
        // Arrange
        var options = new ConsoleDemoOptions
        {
            HandleCancelKeyPress = true,
            ExitOnCancelWhenIdle = false
        };

        var handler = new ConsoleCancelHandler(options, static () => null);
        var cancelHandler = handler.Register();
        var args = CreateCancelEventArgs();

        try
        {
            // Act
            cancelHandler?.Invoke(this, args);

            // Assert
            Assert.True(args.Cancel);
        }
        finally
        {
            if (cancelHandler is not null)
            {
                Console.CancelKeyPress -= cancelHandler;
            }
        }
    }

    private static ConsoleCancelEventArgs CreateCancelEventArgs()
    {
        return (ConsoleCancelEventArgs)Activator.CreateInstance(
            typeof(ConsoleCancelEventArgs),
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            binder: null,
            args: new object[] { ConsoleSpecialKey.ControlC },
            culture: null)!;
    }
}
