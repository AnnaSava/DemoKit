using SavaDev.DemoKit.ConsoleWorker.Args;

namespace SavaDev.DemoKit.ConsoleWorker.Tests.ConsoleWorkerArgsParserTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleArgsParser.Parse"/>.
/// </summary>
/// <remarks>
/// These tests focus on parsing command-line
/// arguments into worker start options.
/// </remarks>
public sealed class Parse_Tests
{
    /// <summary>
    /// Verifies that empty arguments return default options.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Mode defaults to <see cref="WorkerMode.Run"/>.</item>
    /// <item>Interval defaults to 1000.</item>
    /// <item>Exit delay defaults to 5.</item>
    /// <item>Color defaults to <see cref="ConsoleColor.Gray"/>.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenArgsEmpty_ShouldReturnDefaults()
    {
        // Arrange
        var parser = new ConsoleArgsParser();

        // Act
        var result = parser.Parse(Array.Empty<string>());

        // Assert
        Assert.Equal(WorkerMode.Run, result.Options.Mode);
        Assert.Equal(1000, result.Options.Interval);
        Assert.Equal(5, result.Options.ExitAfterSeconds);
        Assert.Equal(ConsoleColor.Gray, result.Options.Color);
    }

    /// <summary>
    /// Verifies that null arguments are treated as empty.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Mode defaults to <see cref="WorkerMode.Run"/>.</item>
    /// <item>Interval defaults to 1000.</item>
    /// <item>Exit delay defaults to 5.</item>
    /// <item>Color defaults to <see cref="ConsoleColor.Gray"/>.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenArgsNull_ShouldReturnDefaults()
    {
        // Arrange
        var parser = new ConsoleArgsParser();

        // Act
        var result = parser.Parse(null);

        // Assert
        Assert.Equal(WorkerMode.Run, result.Options.Mode);
        Assert.Equal(1000, result.Options.Interval);
        Assert.Equal(5, result.Options.ExitAfterSeconds);
        Assert.Equal(ConsoleColor.Gray, result.Options.Color);
    }

    /// <summary>
    /// Verifies that mode, interval, and exit delay are parsed.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Mode is parsed from <c>--mode</c>.</item>
    /// <item>Interval is parsed from <c>--interval</c>.</item>
    /// <item>Exit delay is parsed from <c>--exit-after</c>.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenModeIntervalAndExitProvided_ShouldParseValues()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--mode", "exit",
            "--interval", "250",
            "--exit-after", "7"
        };

        // Act
        var result = parser.Parse(args);

        // Assert
        Assert.Equal(WorkerMode.Exit, result.Options.Mode);
        Assert.Equal(250, result.Options.Interval);
        Assert.Equal(7, result.Options.ExitAfterSeconds);
    }

    /// <summary>
    /// Verifies that color is parsed case-insensitively.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Color is parsed from <c>--color</c>.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenColorProvided_ShouldParseColor()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--color", "cYaN"
        };

        // Act
        var result = parser.Parse(args);

        // Assert
        Assert.Equal(ConsoleColor.Cyan, result.Options.Color);
    }

    /// <summary>
    /// Verifies that missing mode value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenModeValueMissing_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--mode"
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--mode", exception.Message);
    }

    /// <summary>
    /// Verifies that missing interval value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenIntervalValueMissing_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--interval"
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--interval", exception.Message);
    }

    /// <summary>
    /// Verifies that missing exit-after value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenExitAfterValueMissing_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--exit-after"
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--exit-after", exception.Message);
    }

    /// <summary>
    /// Verifies that missing color value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenColorValueMissing_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--color"
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--color", exception.Message);
    }

    /// <summary>
    /// Verifies that an invalid mode value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenModeValueInvalid_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--mode", "not-a-mode"
        };

        // Act
        Assert.Throws<ArgumentException>(
            () => parser.Parse(args));
    }

    /// <summary>
    /// Verifies that an invalid color value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenColorValueInvalid_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--color", "not-a-color"
        };

        // Act
        Assert.Throws<ArgumentException>(
            () => parser.Parse(args));
    }

    /// <summary>
    /// Verifies that an out-of-range color value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenColorValueOutOfRange_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--color", "99"
        };

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--color", exception.Message);
    }

    /// <summary>
    /// Verifies that an invalid interval value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>A <see cref="FormatException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenIntervalValueInvalid_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--interval", "abc"
        };

        // Act
        Assert.Throws<FormatException>(
            () => parser.Parse(args));
    }

    /// <summary>
    /// Verifies that an invalid exit-after value throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>A <see cref="FormatException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenExitAfterValueInvalid_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--exit-after", "oops"
        };

        // Act
        Assert.Throws<FormatException>(
            () => parser.Parse(args));
    }

    /// <summary>
    /// Verifies that zero interval throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentOutOfRangeException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenIntervalZero_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--interval", "0"
        };

        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--interval", exception.Message);
    }

    /// <summary>
    /// Verifies that negative interval throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentOutOfRangeException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenIntervalNegative_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--interval", "-5"
        };

        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--interval", exception.Message);
    }

    /// <summary>
    /// Verifies that zero exit-after throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentOutOfRangeException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenExitAfterZero_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--exit-after", "0"
        };

        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--exit-after", exception.Message);
    }

    /// <summary>
    /// Verifies that negative exit-after throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentOutOfRangeException"/> is thrown.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenExitAfterNegative_ShouldThrow()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--exit-after", "-1"
        };

        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => parser.Parse(args));

        // Assert
        Assert.Contains("--exit-after", exception.Message);
    }

    /// <summary>
    /// Verifies that warnings are returned for unknown arguments.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>Unknown arguments are reported as warnings.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void Parse_WhenUnknownArgumentsProvided_ShouldReturnWarnings()
    {
        // Arrange
        var parser = new ConsoleArgsParser();
        var args = new[]
        {
            "--unknown",
            "value"
        };

        // Act
        var result = parser.Parse(args);

        // Assert
        Assert.Contains("Unknown argument: --unknown", result.Warnings);
        Assert.Contains("Unknown argument: value", result.Warnings);
    }
}
