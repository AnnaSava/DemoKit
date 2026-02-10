namespace SavaDev.DemoKit.ConsoleWorker.Executor;

/// <summary>
/// Defines an execution entry point for worker modes.
/// </summary>
internal interface IWorkerModeExecutor
{
    /// <summary>
    /// Executes a specific worker mode.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process exit code.</returns>
    Task<int> ExecuteAsync(CancellationToken ct);
}
