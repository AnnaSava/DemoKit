using Xunit;

namespace SavaDev.DemoKit.ConsoleWorker.Tests;

/// <summary>
/// Defines a non-parallelized test collection for console-dependent tests.
/// </summary>
[CollectionDefinition("Console", DisableParallelization = true)]
public sealed class ConsoleTestCollection
{
}
