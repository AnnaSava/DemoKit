using SavaDev.DemoKit.ConsoleEngine;

namespace SavaDev.DemoKit.ConsoleWorker.Demo.Scenarios;

/// <summary>
/// Demonstrates spam mode with a customized long message template.
/// </summary>
/// <remarks>
/// This scenario starts the worker in <c>spam</c> mode with a long
/// lorem ipsum template that begins with the message index.
/// </remarks>
public sealed class SpamCustomMessageScenario : IConsoleDemoScenario
{
    /// <inheritdoc />
    public string Name => "Spam (custom message)";

    /// <summary>
    /// Runs the scenario with a customized spam message template.
    /// </summary>
    /// <param name="ct">
    /// A cancellation token that can be used to cancel
    /// the scenario execution.
    /// </param>
    public async Task RunAsync(CancellationToken ct)
    {
        _ = ct;

        Console.WriteLine("Starting in-process worker (spam mode, custom messages)...");
        Console.WriteLine();

        var options = new ConsoleWorkerOptions
        {
            SpamMessageTemplate =
                "{0}. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
        };

        var worker = new ConsoleDemoWorker(
            options: options,
            args: new[] { "--mode", "spam", "--interval", "75" });

        await worker.RunAsync();
    }
}
