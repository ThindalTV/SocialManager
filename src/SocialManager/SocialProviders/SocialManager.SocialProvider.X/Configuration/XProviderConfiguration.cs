using System.ComponentModel.DataAnnotations;
using SocialProvider;

namespace SocialManager.SocialProvider.X.Configuration;

/// <summary>
/// Configuration settings for X (Twitter) API integration.
/// </summary>
public class XProviderConfiguration : ISocialProviderConfiguration
{
    public const string SectionName = "XProvider";

    /// <inheritdoc />
    public bool Active { get; set; } = true;

    /// <inheritdoc />
    public string Platform { get; set; } = "X";

    /// <summary>
    /// Gets or sets the API Key (Consumer Key) from X Developer Portal.
    /// </summary>
    [Required]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API Secret (Consumer Secret) from X Developer Portal.
    /// </summary>
    [Required]
    public string ApiSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Access Token for the authenticated user.
    /// </summary>
    [Required]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Access Token Secret for the authenticated user.
    /// </summary>
    [Required]
    public string AccessTokenSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Bearer Token for OAuth 2.0 authentication (optional, used for read operations).
    /// </summary>
    public string? BearerToken { get; set; }

    /// <summary>
    /// Gets or sets whether to enable retry logic for rate-limited requests.
    /// </summary>
    public bool EnableRetryOnRateLimit { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Validates the configuration.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new InvalidOperationException("XProvider ApiKey is required.");

        if (string.IsNullOrWhiteSpace(ApiSecret))
            throw new InvalidOperationException("XProvider ApiSecret is required.");

        if (string.IsNullOrWhiteSpace(AccessToken))
            throw new InvalidOperationException("XProvider AccessToken is required.");

        if (string.IsNullOrWhiteSpace(AccessTokenSecret))
            throw new InvalidOperationException("XProvider AccessTokenSecret is required.");

        if (MaxRetryAttempts < 0)
            throw new InvalidOperationException("XProvider MaxRetryAttempts must be non-negative.");
    }
}
