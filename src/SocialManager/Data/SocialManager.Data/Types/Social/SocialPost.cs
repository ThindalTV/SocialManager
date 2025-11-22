namespace SocialManager.Data.Types.Social;

/// <summary>
/// Represents a social media post containing content and metadata for publishing to a specific platform.
/// </summary>
public class SocialPost
{
    /// <summary>
    /// Gets or sets the social media platform identifier (e.g., "Twitter", "Facebook", "LinkedIn").
    /// </summary>
    public required string Platform { get; set; }

    /// <summary>
    /// Gets or sets the text content of the post.
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// Gets or sets the optional URL to media content (image, video, etc.) to be included with the post.
    /// </summary>
    public string? MediaUrl { get; set; }

    /// <summary>
    /// Gets or sets the optional URL to be included as a link in the post.
    /// </summary>
    public string? LinkUrl { get; set; }
}
