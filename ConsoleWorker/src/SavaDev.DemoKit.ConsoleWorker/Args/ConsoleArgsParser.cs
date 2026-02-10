namespace SavaDev.DemoKit.ConsoleWorker.Args;

/// <summary>
/// Parses command-line arguments for the demo worker.
/// </summary>
internal sealed class ConsoleArgsParser
{
    /// <summary>
    /// Parses command-line arguments and returns start options with warnings.
    /// </summary>
    /// <param name="args">Command-line arguments to parse.</param>
    /// <returns>The parsed options and warnings.</returns>
    public ArgsParseResult Parse(string[]? args)
    {
        var argsArray = args ?? Array.Empty<string>();
        var mode = WorkerMode.Run;
        var interval = 1000;
        var exitAfter = 5;
        var color = ConsoleColor.Gray;
        var warnings = new List<string>();

        for (var i = 0; i < argsArray.Length; i++)
        {
            switch (argsArray[i])
            {
                case "--mode":
                    mode = Enum.Parse<WorkerMode>(
                        GetNextValue(argsArray, ref i, "--mode", nameof(args)),
                        ignoreCase: true);
                    break;

                case "--interval":
                    interval = int.Parse(
                        GetNextValue(argsArray, ref i, "--interval", nameof(args)));
                    break;

                case "--exit-after":
                    exitAfter = int.Parse(
                        GetNextValue(argsArray, ref i, "--exit-after", nameof(args)));
                    break;

                case "--color":
                    color = Enum.Parse<ConsoleColor>(
                        GetNextValue(argsArray, ref i, "--color", nameof(args)),
                        ignoreCase: true);
                    break;

                default:
                    warnings.Add($"Unknown argument: {argsArray[i]}");
                    break;
            }
        }

        ValidateColor(color);
        ValidatePositiveValues(interval, exitAfter);

        return new ArgsParseResult
        {
            Options = new ArgsOptions
            {
                Mode = mode,
                Interval = interval,
                ExitAfterSeconds = exitAfter,
                Color = color
            },
            Warnings = warnings
        };
    }

    /// <summary>
    /// Validates numeric options for positive values.
    /// </summary>
    /// <param name="interval">Message interval in milliseconds.</param>
    /// <param name="exitAfterSeconds">Exit delay in seconds.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when values are less than or equal to zero.
    /// </exception>
    private static void ValidatePositiveValues(int interval, int exitAfterSeconds)
    {
        if (interval <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(interval),
                "Invalid value for --interval. The value must be greater than zero.");
        }

        if (exitAfterSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(exitAfterSeconds),
                "Invalid value for --exit-after. The value must be greater than zero.");
        }
    }

    private static void ValidateColor(ConsoleColor color)
    {
        if (!Enum.IsDefined(typeof(ConsoleColor), color))
        {
            throw new ArgumentException(
                "Invalid value for --color.",
                nameof(color));
        }
    }

    /// <summary>
    /// Gets the next argument value or throws when it is missing.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <param name="index">Current argument index.</param>
    /// <param name="optionName">Option that requires a value.</param>
    /// <param name="paramName">Parameter name for the exception.</param>
    /// <returns>The next argument value.</returns>
    private static string GetNextValue(
        string[] args,
        ref int index,
        string optionName,
        string paramName)
    {
        if (index + 1 >= args.Length)
        {
            throw new ArgumentException(
                $"Missing value for {optionName}.",
                paramName);
        }

        index++;
        return args[index];
    }
}
