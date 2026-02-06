using System.Reflection;

namespace SavaDev.DemoKit.ConsoleEngine.Services;

/// <summary>
/// Provides version information for the current host application.
/// </summary>
/// <remarks>
/// This provider determines the version of the executable that hosts
/// the demo engine.
///
/// The version resolution is intentionally simple and explicit:
/// <list type="bullet">
/// <item>
/// The version is taken from
/// <see cref="AssemblyInformationalVersionAttribute"/> of the entry
/// assembly.
/// </item>
/// <item>
/// If the informational version attribute is not present, a fallback
/// value is used.
/// </item>
/// </list>
///
/// In SDK-style projects, the .NET SDK may automatically generate
/// <see cref="AssemblyInformationalVersionAttribute"/> with a default
/// value (for example, <c>1.0.0</c>) even when no version is explicitly
/// specified by the application author.
///
/// This provider does not attempt to distinguish between explicitly
/// defined and SDK-generated informational versions. It reports the
/// value as observed at runtime.
/// </remarks>
internal sealed class AppVersionProvider
{
    /// <summary>
    /// Gets the application version of the host executable.
    /// </summary>
    /// <returns>
    /// The informational version reported by the entry assembly,
    /// or <c>"0.0.0"</c> when no informational version is available.
    /// </returns>
    /// <remarks>
    /// This method reads the value of
    /// <see cref="AssemblyInformationalVersionAttribute"/> from the entry
    /// assembly and returns it as-is.
    ///
    /// No attempt is made to infer author intent or to filter out
    /// SDK-generated default versions. If the host application relies on
    /// a specific version value, it is expected to define it explicitly
    /// in the project configuration.
    /// </remarks>
    public string GetVersion()
    {
        return GetHostInformationalVersion() ?? "0.0.0";
    }

    /// <summary>
    /// Attempts to retrieve the informational version of the host
    /// application from its entry assembly.
    /// </summary>
    /// <returns>
    /// The value of <see cref="AssemblyInformationalVersionAttribute"/>
    /// if present; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// The entry assembly represents the executable that started the
    /// current process.
    ///
    /// This method returns <c>null</c> when:
    /// <list type="bullet">
    /// <item>The entry assembly cannot be determined.</item>
    /// <item>The informational version attribute is not defined.</item>
    /// <item>An unexpected reflection error occurs.</item>
    /// </list>
    ///
    /// Exceptions are intentionally suppressed, as version detection
    /// should never interfere with demo execution.
    /// </remarks>
    private static string? GetHostInformationalVersion()
    {
        try
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly is null)
            {
                return null;
            }

            return entryAssembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;
        }
        catch
        {
            return null;
        }
    }
}
