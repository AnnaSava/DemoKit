namespace SavaDev.DemoKit.ConsoleEngine.Assets;

/// <summary>
/// Provides helper methods for resolving paths to external asset executables
/// used by console demo scenarios.
/// </summary>
/// <remarks>
/// This helper encapsulates a common resolution strategy for demo assets:
/// <list type="bullet">
/// <item>
/// Prefer executables located next to the currently running demo binary
/// (binary distribution scenario).
/// </item>
/// <item>
/// Fall back to resolving executables from development build output
/// directories when running from source.
/// </item>
/// </list>
///
/// The resolver is intentionally implemented as a static utility to keep
/// demo scenarios simple and free of infrastructure dependencies.
/// </remarks>
public static class ConsoleAssetResolver
{
    /// <summary>
    /// Resolves the absolute path to an external asset executable required
    /// by a console demo scenario.
    /// </summary>
    /// <param name="assetProjectName">
    /// The name of the asset project, used to derive the executable file name.
    /// </param>
    /// <param name="assetBaseRelativePath">
    /// The base relative path to the asset project's <c>bin</c> directory,
    /// excluding build configuration and target framework.
    /// </param>
    /// <param name="targetFramework">
    /// The target framework moniker (TFM) used when building the asset project
    /// (for example, <c>net8.0</c>).
    /// </param>
    /// <returns>
    /// The absolute path to the resolved asset executable.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the asset executable cannot be found in any of the
    /// expected runtime or development locations.
    /// </exception>
    public static string ResolveAssetExecutablePath(
        string assetProjectName,
        string assetBaseRelativePath,
        string targetFramework)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetProjectName, nameof(assetProjectName));
        ArgumentException.ThrowIfNullOrWhiteSpace(assetBaseRelativePath, nameof(assetBaseRelativePath));
        ArgumentException.ThrowIfNullOrWhiteSpace(targetFramework, nameof(targetFramework));

        var info = new AssetExecutableInfo(assetProjectName, assetBaseRelativePath, targetFramework);
        return ResolvePath(info);
    }

    /// <summary>
    /// Resolves the full path to an asset executable used by a console demo.
    /// </summary>
    /// <remarks>
    /// The resolution follows a two-stage strategy:
    /// <list type="number">
    /// <item>
    /// First, the asset executable is searched next to the currently
    /// executing demo binary (runtime / binary distribution scenario).
    /// </item>
    /// <item>
    /// If not found, the executable is resolved from the development
    /// build output directory using the current build configuration
    /// (development-from-source scenario).
    /// </item>
    /// </list>
    ///
    /// If neither location contains the executable, a developer-oriented
    /// instruction is printed and an exception is thrown.
    /// </remarks>
    /// <returns>
    /// The absolute path to the resolved asset executable.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the asset executable cannot be found in any of the
    /// expected locations.
    /// </exception>
    private static string ResolvePath(AssetExecutableInfo info)
    {
        var workerExeName = ResolveWorkerExecutableName(info.AssetProjectName);

        if (TryResolveRuntimeWorkerPath(workerExeName, out var runtimePath))
            return runtimePath;

        PrintRuntimeFallbackMessage(runtimePath);

        var configuration = ResolveConfiguration();

        if (TryResolveDevelopmentWorkerPath(info, workerExeName, configuration, out var devPath))
            return devPath;

        PrintWorkerNotFoundInstructions(info, devPath, configuration);

        throw new FileNotFoundException(
            "Demo asset executable was not found.",
            devPath);
    }

    /// <summary>
    /// Resolves the platform-specific file name of an asset executable
    /// based on the project name.
    /// </summary>
    /// <param name="projectName">
    /// The asset project name used as the base executable name.
    /// </param>
    /// <returns>
    /// The executable file name, including the <c>.exe</c> extension
    /// on Windows.
    /// </returns>
    private static string ResolveWorkerExecutableName(string projectName)
    {
        return OperatingSystem.IsWindows()
            ? $"{projectName}.exe"
            : projectName;
    }

    /// <summary>
    /// Attempts to resolve an asset executable located next to the
    /// currently executing demo binary.
    /// </summary>
    /// <param name="workerExeName">
    /// The file name of the asset executable.
    /// </param>
    /// <param name="workerPath">
    /// When this method returns <c>true</c>, contains the resolved
    /// absolute path to the executable.
    /// </param>
    /// <returns>
    /// <c>true</c> if the executable exists at the runtime location;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This lookup supports binary demo distributions where the demo
    /// executable and its asset executables are deployed side by side.
    /// </remarks>
    private static bool TryResolveRuntimeWorkerPath(
        string workerExeName,
        out string workerPath)
    {
        workerPath = Path.Combine(
            AppContext.BaseDirectory,
            workerExeName);

        return File.Exists(workerPath);
    }

    /// <summary>
    /// Resolves the build configuration used for the current demo run.
    /// </summary>
    /// <remarks>
    /// The configuration is determined from environment variables
    /// (<c>DOTNET_CONFIGURATION</c> or <c>CONFIGURATION</c>).
    /// If no value is provided, <c>Debug</c> is used as a fallback.
    /// </remarks>
    /// <returns>
    /// The resolved build configuration name.
    /// </returns>
    private static string ResolveConfiguration()
    {
        return
            Environment.GetEnvironmentVariable("DOTNET_CONFIGURATION")
            ?? Environment.GetEnvironmentVariable("CONFIGURATION")
            ?? "Debug";
    }

    /// <summary>
    /// Attempts to resolve an asset executable from the development
    /// build output directory.
    /// </summary>
    /// <param name="info">
    /// Metadata describing the asset project and its expected layout
    /// in the development build output.
    /// </param>
    /// <param name="workerExeName">
    /// The file name of the asset executable.
    /// </param>
    /// <param name="configuration">
    /// The build configuration name (for example, <c>Debug</c>
    /// or <c>Release</c>).
    /// </param>
    /// <param name="workerPath">
    /// When this method returns <c>true</c>, contains the resolved
    /// absolute path to the executable.
    /// </param>
    /// <returns>
    /// <c>true</c> if the executable exists in the development
    /// build output directory; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This lookup supports development scenarios where the demo and
    /// its asset projects are built from source and reside in their
    /// respective <c>bin</c> directories.
    /// </remarks>
    private static bool TryResolveDevelopmentWorkerPath(AssetExecutableInfo info,
        string workerExeName,
        string configuration,
        out string workerPath)
    {
        workerPath = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                info.AssetBaseRelativePath,
                "bin",
                configuration,
                info.TargetFramework,
                workerExeName));

        return File.Exists(workerPath);
    }

    /// <summary>
    /// Prints a short informational message indicating that the asset
    /// executable was not found next to the demo binary and that the
    /// resolver is falling back to the development build output.
    /// </summary>
    /// <param name="runtimePath">
    /// The runtime path where the asset executable was expected to be
    /// located.
    /// </param>
    /// <remarks>
    /// This message is informational only and does not indicate an error.
    /// It is intended to explain the resolution flow to users running
    /// a binary demo, without producing unnecessary noise for developers.
    /// </remarks>
    private static void PrintRuntimeFallbackMessage(string runtimePath)
    {
        Console.WriteLine(
            $"Asset executable was not found at runtime location: {runtimePath}");
        Console.WriteLine(
            "Falling back to development build output.");
        Console.WriteLine();
    }

    /// <summary>
    /// Prints developer-oriented instructions explaining how to make
    /// an asset executable available when it cannot be resolved.
    /// </summary>
    /// <param name="info">
    /// Metadata describing the asset project and its expected build layout.
    /// </param>
    /// <param name="expectedPath">
    /// The full path where the asset executable was expected to be
    /// located in the development build output.
    /// </param>
    /// <param name="configuration">
    /// The build configuration that was used when resolving the
    /// expected executable path.
    /// </param>
    /// <remarks>
    /// This helper is intended for development scenarios only.
    /// It assumes the reader has access to the source code and can
    /// build asset projects locally.
    ///
    /// Binary demo users are expected to have asset executables
    /// deployed alongside the demo binary and should not normally
    /// encounter this message.
    /// </remarks>
    private static void PrintWorkerNotFoundInstructions(AssetExecutableInfo info,
        string expectedPath,
        string configuration)
    {
        Console.WriteLine("Asset executable was not found.");
        Console.WriteLine();
        Console.WriteLine("This demo scenario requires a locally built asset executable.");
        Console.WriteLine();
        Console.WriteLine("To proceed, do one of the following:");
        Console.WriteLine();
        Console.WriteLine("1. Build the asset project:");
        Console.WriteLine($"   {info.AssetProjectName}");
        Console.WriteLine($"   using '{configuration}' configuration and the '{info.TargetFramework}' target framework.");
        Console.WriteLine();
        Console.WriteLine("2. Ensure the executable exists at:");
        Console.WriteLine($"   {expectedPath}");
        Console.WriteLine();
        Console.WriteLine("3. Or update the demo code to explicitly specify the path ");
        Console.WriteLine("to the asset executable (and related parameters if needed).");
        Console.WriteLine();
    }
}
