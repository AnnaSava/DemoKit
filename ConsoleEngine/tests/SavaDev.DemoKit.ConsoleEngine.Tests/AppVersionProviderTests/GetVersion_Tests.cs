using System.Reflection;
using SavaDev.DemoKit.ConsoleEngine.Services;

namespace SavaDev.DemoKit.ConsoleEngine.Tests.AppVersionProviderTests;

/// <summary>
/// Contains tests that verify the behavior of
/// <see cref="AppVersionProvider.GetVersion"/>.
/// </summary>
/// <remarks>
/// These tests focus on ensuring a usable
/// version string is returned.
/// </remarks>
public sealed class GetVersion_Tests
{
    /// <summary>
    /// Verifies that version retrieval returns
    /// a non-empty string.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>A non-null version string is returned.</item>
    /// <item>The version string is not whitespace.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void GetVersion_ShouldReturnNonEmptyVersion()
    {
        // Arrange
        var provider = new AppVersionProvider();

        // Act
        var version = provider.GetVersion();

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(version));
    }

    /// <summary>
    /// Verifies that the provider returns the
    /// entry assembly informational version when available.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// <list type="bullet">
    /// <item>The returned version matches the entry assembly attribute.</item>
    /// <item>The fallback value is used when no attribute exists.</item>
    /// </list>
    /// </remarks>
    [Fact]
    public void GetVersion_ShouldMatchEntryAssemblyInformationalVersion()
    {
        // Arrange
        var expected = GetEntryAssemblyInformationalVersion() ?? "0.0.0";
        var provider = new AppVersionProvider();

        // Act
        var version = provider.GetVersion();

        // Assert
        Assert.Equal(expected, version);
    }

    private static string? GetEntryAssemblyInformationalVersion()
    {
        var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
        if (entryAssembly is null)
        {
            return null;
        }

        var attribute = entryAssembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        return attribute?.InformationalVersion;
    }
}
