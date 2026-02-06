using SavaDev.DemoKit.ConsoleEngine.Assets;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.ConsoleAssetResolverTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="ConsoleAssetResolver.ResolveAssetExecutablePath(string, string, string)"/>.
/// </summary>
/// <remarks>
/// These tests validate runtime resolution, development fallback,
/// and error reporting for missing assets.
/// </remarks>
[Collection("Console")]
public sealed class ResolveAssetExecutablePath_Tests
{
    /// <summary>
    /// Verifies that an executable located next to the
    /// demo binary is returned immediately.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The runtime executable path is returned.</item>
    /// <item>No development fallback is required.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenRuntimeExecutableExists_ShouldReturnRuntimePath()
    {
        // Arrange
        var assetProjectName = $"RuntimeAsset_{Guid.NewGuid():N}";
        var assetBaseRelativePath = "assets";
        var targetFramework = "net8.0";

        var workerExeName = ResolveWorkerExecutableName(assetProjectName);
        var runtimePath = Path.Combine(AppContext.BaseDirectory, workerExeName);

        CreateEmptyFile(runtimePath);

        try
        {
            // Act
            var result = ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName,
                assetBaseRelativePath,
                targetFramework);

            // Assert
            Assert.Equal(runtimePath, result);
        }
        finally
        {
            DeleteFileIfExists(runtimePath);
        }
    }

    /// <summary>
    /// Verifies that the resolver falls back to the
    /// development output directory when the runtime
    /// executable is missing.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The development executable path is returned.</item>
    /// <item>The path uses the resolved configuration and TFM.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenRuntimeMissing_ShouldReturnDevelopmentPath()
    {
        // Arrange
        var assetProjectName = $"DevAsset_{Guid.NewGuid():N}";
        var assetBaseRelativePath = Path.Combine(
            "assets",
            "ConsoleAssetResolverTests",
            Guid.NewGuid().ToString("N"));
        var targetFramework = "net8.0";
        var configuration = $"TestConfig_{Guid.NewGuid():N}";

        var workerExeName = ResolveWorkerExecutableName(assetProjectName);
        var devPath = BuildDevelopmentPath(
            assetBaseRelativePath,
            configuration,
            targetFramework,
            workerExeName);

        CreateEmptyFile(devPath);

        var originalDotNetConfig = Environment.GetEnvironmentVariable("DOTNET_CONFIGURATION");
        var originalConfig = Environment.GetEnvironmentVariable("CONFIGURATION");

        Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", configuration);
        Environment.SetEnvironmentVariable("CONFIGURATION", null);

        try
        {
            // Act
            var result = ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName,
                assetBaseRelativePath,
                targetFramework);

            // Assert
            Assert.Equal(devPath, result);
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", originalDotNetConfig);
            Environment.SetEnvironmentVariable("CONFIGURATION", originalConfig);

            DeleteDirectoryIfExists(
                Path.Combine(AppContext.BaseDirectory, assetBaseRelativePath));
        }
    }

    /// <summary>
    /// Verifies that missing executables result in
    /// a detailed error message and exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>A <see cref="FileNotFoundException"/> is thrown.</item>
    /// <item>Developer instructions are written to the console.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenExecutableMissing_ShouldThrowAndPrintInstructions()
    {
        // Arrange
        var assetProjectName = $"MissingAsset_{Guid.NewGuid():N}";
        var assetBaseRelativePath = Path.Combine(
            "assets",
            "ConsoleAssetResolverTests",
            Guid.NewGuid().ToString("N"));
        var targetFramework = "net8.0";
        var configuration = $"MissingConfig_{Guid.NewGuid():N}";

        var workerExeName = ResolveWorkerExecutableName(assetProjectName);
        var expectedDevPath = BuildDevelopmentPath(
            assetBaseRelativePath,
            configuration,
            targetFramework,
            workerExeName);

        var originalDotNetConfig = Environment.GetEnvironmentVariable("DOTNET_CONFIGURATION");
        var originalConfig = Environment.GetEnvironmentVariable("CONFIGURATION");

        Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", configuration);
        Environment.SetEnvironmentVariable("CONFIGURATION", null);

        try
        {
            // Act
            var output = CaptureConsoleOutput(() =>
            {
                var exception = Assert.Throws<FileNotFoundException>(() =>
                    ConsoleAssetResolver.ResolveAssetExecutablePath(
                        assetProjectName,
                        assetBaseRelativePath,
                        targetFramework));

                Assert.Equal(expectedDevPath, exception.FileName);
            });

            // Assert
            Assert.Contains("Asset executable was not found.", output);
            Assert.Contains("Build the asset project:", output);
            Assert.Contains(assetProjectName, output);
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", originalDotNetConfig);
            Environment.SetEnvironmentVariable("CONFIGURATION", originalConfig);
        }
    }

    /// <summary>
    /// Verifies that the resolver prefers the
    /// DOTNET_CONFIGURATION environment variable
    /// over CONFIGURATION when both are provided.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The DOTNET_CONFIGURATION value is used.</item>
    /// <item>The resolved path points to that configuration.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenBothConfigurationsProvided_ShouldPreferDotNetConfiguration()
    {
        // Arrange
        var assetProjectName = $"PreferConfig_{Guid.NewGuid():N}";
        var assetBaseRelativePath = Path.Combine(
            "assets",
            "ConsoleAssetResolverTests",
            Guid.NewGuid().ToString("N"));
        var targetFramework = "net8.0";
        var dotNetConfiguration = $"DotNet_{Guid.NewGuid():N}";
        var configuration = $"Config_{Guid.NewGuid():N}";

        var workerExeName = ResolveWorkerExecutableName(assetProjectName);
        var expectedPath = BuildDevelopmentPath(
            assetBaseRelativePath,
            dotNetConfiguration,
            targetFramework,
            workerExeName);

        CreateEmptyFile(expectedPath);

        var originalDotNetConfig = Environment.GetEnvironmentVariable("DOTNET_CONFIGURATION");
        var originalConfig = Environment.GetEnvironmentVariable("CONFIGURATION");

        Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", dotNetConfiguration);
        Environment.SetEnvironmentVariable("CONFIGURATION", configuration);

        try
        {
            // Act
            var result = ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName,
                assetBaseRelativePath,
                targetFramework);

            // Assert
            Assert.Equal(expectedPath, result);
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", originalDotNetConfig);
            Environment.SetEnvironmentVariable("CONFIGURATION", originalConfig);

            DeleteDirectoryIfExists(
                Path.Combine(AppContext.BaseDirectory, assetBaseRelativePath));
        }
    }

    /// <summary>
    /// Verifies that the resolver uses the
    /// CONFIGURATION environment variable
    /// when DOTNET_CONFIGURATION is not provided.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The CONFIGURATION value is used.</item>
    /// <item>The resolved path points to that configuration.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenDotNetConfigurationMissing_ShouldUseConfiguration()
    {
        // Arrange
        var assetProjectName = $"ConfigOnly_{Guid.NewGuid():N}";
        var assetBaseRelativePath = Path.Combine(
            "assets",
            "ConsoleAssetResolverTests",
            Guid.NewGuid().ToString("N"));
        var targetFramework = "net8.0";
        var configuration = $"Config_{Guid.NewGuid():N}";

        var workerExeName = ResolveWorkerExecutableName(assetProjectName);
        var expectedPath = BuildDevelopmentPath(
            assetBaseRelativePath,
            configuration,
            targetFramework,
            workerExeName);

        CreateEmptyFile(expectedPath);

        var originalDotNetConfig = Environment.GetEnvironmentVariable("DOTNET_CONFIGURATION");
        var originalConfig = Environment.GetEnvironmentVariable("CONFIGURATION");

        Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", null);
        Environment.SetEnvironmentVariable("CONFIGURATION", configuration);

        try
        {
            // Act
            var result = ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName,
                assetBaseRelativePath,
                targetFramework);

            // Assert
            Assert.Equal(expectedPath, result);
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", originalDotNetConfig);
            Environment.SetEnvironmentVariable("CONFIGURATION", originalConfig);

            DeleteDirectoryIfExists(
                Path.Combine(AppContext.BaseDirectory, assetBaseRelativePath));
        }
    }

    /// <summary>
    /// Verifies that a missing runtime executable
    /// emits the fallback message before resolving
    /// the development path.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The fallback message is printed.</item>
    /// <item>The development path is returned.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenRuntimeMissing_ShouldPrintFallbackMessage()
    {
        // Arrange
        var assetProjectName = $"Fallback_{Guid.NewGuid():N}";
        var assetBaseRelativePath = Path.Combine(
            "assets",
            "ConsoleAssetResolverTests",
            Guid.NewGuid().ToString("N"));
        var targetFramework = "net8.0";
        var configuration = $"FallbackConfig_{Guid.NewGuid():N}";

        var workerExeName = ResolveWorkerExecutableName(assetProjectName);
        var expectedPath = BuildDevelopmentPath(
            assetBaseRelativePath,
            configuration,
            targetFramework,
            workerExeName);

        CreateEmptyFile(expectedPath);

        var originalDotNetConfig = Environment.GetEnvironmentVariable("DOTNET_CONFIGURATION");
        var originalConfig = Environment.GetEnvironmentVariable("CONFIGURATION");

        Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", configuration);
        Environment.SetEnvironmentVariable("CONFIGURATION", null);

        try
        {
            // Act
            var output = CaptureConsoleOutput(() =>
                ConsoleAssetResolver.ResolveAssetExecutablePath(
                    assetProjectName,
                    assetBaseRelativePath,
                    targetFramework));

            // Assert
            Assert.Contains("Falling back to development build output.", output);
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", originalDotNetConfig);
            Environment.SetEnvironmentVariable("CONFIGURATION", originalConfig);

            DeleteDirectoryIfExists(
                Path.Combine(AppContext.BaseDirectory, assetBaseRelativePath));
        }
    }

    /// <summary>
    /// Verifies that a missing configuration environment
    /// defaults to the Debug build output.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The Debug configuration path is used.</item>
    /// <item>The executable is resolved from that path.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenConfigurationMissing_ShouldUseDebug()
    {
        // Arrange
        var assetProjectName = $"DebugFallback_{Guid.NewGuid():N}";
        var assetBaseRelativePath = Path.Combine(
            "assets",
            "ConsoleAssetResolverTests",
            Guid.NewGuid().ToString("N"));
        var targetFramework = "net8.0";

        var workerExeName = ResolveWorkerExecutableName(assetProjectName);
        var expectedPath = BuildDevelopmentPath(
            assetBaseRelativePath,
            "Debug",
            targetFramework,
            workerExeName);

        CreateEmptyFile(expectedPath);

        var originalDotNetConfig = Environment.GetEnvironmentVariable("DOTNET_CONFIGURATION");
        var originalConfig = Environment.GetEnvironmentVariable("CONFIGURATION");

        Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", null);
        Environment.SetEnvironmentVariable("CONFIGURATION", null);

        try
        {
            // Act
            var result = ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName,
                assetBaseRelativePath,
                targetFramework);

            // Assert
            Assert.Equal(expectedPath, result);
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_CONFIGURATION", originalDotNetConfig);
            Environment.SetEnvironmentVariable("CONFIGURATION", originalConfig);

            DeleteDirectoryIfExists(
                Path.Combine(AppContext.BaseDirectory, assetBaseRelativePath));
        }
    }

    /// <summary>
    /// Verifies that a null asset project name
    /// is rejected with an argument error.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// <item>The parameter name is reported.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenProjectNameIsNull_ShouldThrow()
    {
        // Arrange
        string? assetProjectName = null;

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName!,
                "assets",
                "net8.0"));

        // Assert
        Assert.Equal("assetProjectName", exception.ParamName);
    }

    /// <summary>
    /// Verifies that an empty base relative path
    /// is rejected with an argument error.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// <item>The parameter name is reported.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenBasePathIsEmpty_ShouldThrow()
    {
        // Arrange
        var assetProjectName = "Asset";

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName,
                "",
                "net8.0"));

        // Assert
        Assert.Equal("assetBaseRelativePath", exception.ParamName);
    }

    /// <summary>
    /// Verifies that a missing target framework
    /// is rejected with an argument error.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>An <see cref="ArgumentException"/> is thrown.</item>
    /// <item>The parameter name is reported.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void ResolveAssetExecutablePath_WhenTargetFrameworkIsWhitespace_ShouldThrow()
    {
        // Arrange
        var assetProjectName = "Asset";

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            ConsoleAssetResolver.ResolveAssetExecutablePath(
                assetProjectName,
                "assets",
                " "));

        // Assert
        Assert.Equal("targetFramework", exception.ParamName);
    }

    /// <summary>
    /// Resolves the platform-specific asset executable name.
    /// </summary>
    /// <param name="projectName">The asset project name.</param>
    /// <returns>The platform-specific executable name.</returns>
    private static string ResolveWorkerExecutableName(string projectName)
    {
        return OperatingSystem.IsWindows()
            ? $"{projectName}.exe"
            : projectName;
    }

    /// <summary>
    /// Builds the expected development output path for an asset executable.
    /// </summary>
    /// <param name="assetBaseRelativePath">Base relative path to the asset bin directory.</param>
    /// <param name="configuration">Build configuration name.</param>
    /// <param name="targetFramework">Target framework moniker.</param>
    /// <param name="workerExeName">Executable file name.</param>
    /// <returns>The absolute path to the development output.</returns>
    private static string BuildDevelopmentPath(
        string assetBaseRelativePath,
        string configuration,
        string targetFramework,
        string workerExeName)
    {
        return Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                assetBaseRelativePath,
                "bin",
                configuration,
                targetFramework,
                workerExeName));
    }

    /// <summary>
    /// Creates an empty file and its parent directory.
    /// </summary>
    /// <param name="path">The file path to create.</param>
    private static void CreateEmptyFile(string path)
    {
        var directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(path, string.Empty);
    }

    /// <summary>
    /// Deletes a file if it exists.
    /// </summary>
    /// <param name="path">The file path to remove.</param>
    private static void DeleteFileIfExists(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    /// <summary>
    /// Deletes a directory if it exists.
    /// </summary>
    /// <param name="path">The directory path to remove.</param>
    private static void DeleteDirectoryIfExists(string path)
    {
        if (Directory.Exists(path))
            Directory.Delete(path, recursive: true);
    }

    /// <summary>
    /// Captures console output while executing a callback.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The captured output.</returns>
    private static string CaptureConsoleOutput(Action action)
    {
        var originalOut = Console.Out;

        using var buffer = new StringWriter();
        Console.SetOut(TextWriter.Synchronized(buffer));

        try
        {
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        return buffer.ToString();
    }
}
