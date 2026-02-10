namespace SavaDev.DemoKit.ConsoleEngine.Assets;

/// <summary>
/// Represents metadata required to resolve the executable
/// of an external asset used by a console demo.
/// </summary>
/// <remarks>
/// This type groups together asset-related parameters that describe
/// how and where an asset executable is expected to be built and located.
///
/// It is used as a simple data carrier to keep resolver method signatures
/// concise and to make the resolution logic easier to read and maintain.
/// </remarks>
internal class AssetExecutableInfo
{
    /// <summary>
    /// Gets the asset project name, which is used as the base
    /// name of the executable file.
    /// </summary>
    public string AssetProjectName { get; }

    /// <summary>
    /// Gets the base relative path to the asset project's
    /// <c>bin</c> directory, excluding build configuration
    /// and target framework.
    /// </summary>
    public string AssetBaseRelativePath { get; }

    /// <summary>
    /// Gets the target framework moniker (TFM) used when
    /// building the asset project.
    /// </summary>
    public string TargetFramework { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssetExecutableInfo"/> class.
    /// </summary>
    /// <param name="assetProjectName">
    /// The name of the asset project, used to derive the executable file name.
    /// </param>
    /// <param name="assetBaseRelativePath">
    /// The base relative path to the asset project's <c>bin</c> directory,
    /// excluding build configuration and target framework.
    /// </param>
    /// <param name="targetFramework">
    /// The target framework moniker (TFM) used when building the asset project.
    /// </param>
    public AssetExecutableInfo(
        string assetProjectName,
        string assetBaseRelativePath,
        string targetFramework)
    {
        AssetProjectName = assetProjectName;
        AssetBaseRelativePath = assetBaseRelativePath;
        TargetFramework = targetFramework;
    }
}
