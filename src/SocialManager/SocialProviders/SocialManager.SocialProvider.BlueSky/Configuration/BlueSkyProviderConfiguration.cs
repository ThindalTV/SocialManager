using System.ComponentModel.DataAnnotations;
using SocialProvider;

namespace SocialManager.SocialProvider.BlueSky.Configuration;

/// <summary>
/// Configuration settings for BlueSky AT Protocol integration.
/// </summary>
public class BlueSkyProviderConfiguration : ISocialProviderConfiguration
{
    public const string SectionName = "BlueSkyProvider";

    /// <inheritdoc />
    public bool Active { get; set; } = true;

    /// <inheritdoc />
    public string Platform { get; set; } = "BlueSky";

    /// <summary>
    /// Gets or sets the BlueSky identifier (handle or email) for authentication.
    /// </summary>
    [Required]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the BlueSky app password for authentication.
    /// </summary>
    [Required]
    public string AppPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PDS (Personal Data Server) endpoint URL.
    /// Default is the official BlueSky PDS.
    /// </summary>
    public string PdsUrl { get; set; } = "https://bsky.social";

    /// <summary>
    /// Gets or sets whether to enable retry logic for rate-limited requests.
    /// </summary>
    public bool EnableRetryOnRateLimit { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets the timeout in seconds for API requests.
    /// </summary>
    public int RequestTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Validates the configuration.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Identifier))
            throw new InvalidOperationException("BlueSkyProvider Identifier is required.");

        if (string.IsNullOrWhiteSpace(AppPassword))
            throw new InvalidOperationException("BlueSkyProvider AppPassword is required.");

        if (string.IsNullOrWhiteSpace(PdsUrl))
            throw new InvalidOperationException("BlueSkyProvider PdsUrl is required.");

        if (MaxRetryAttempts < 0)
            throw new InvalidOperationException("BlueSkyProvider MaxRetryAttempts must be non-negative.");

        if (RequestTimeoutSeconds <= 0)
            throw new InvalidOperationException("BlueSkyProvider RequestTimeoutSeconds must be positive.");
    }
}
