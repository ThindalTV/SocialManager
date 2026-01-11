namespace SocialProvider;

/// <summary>
/// Base configuration interface for all social provider implementations.
/// </summary>
public interface ISocialProviderConfiguration
{
    /// <summary>
    /// Gets or sets whether this social provider is enabled.
    /// </summary>
    bool Active { get; set; }
}
