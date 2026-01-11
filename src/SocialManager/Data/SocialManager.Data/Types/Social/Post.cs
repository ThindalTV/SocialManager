namespace SocialManager.Data.Types.Social;

/// <summary>
/// Represents a social media post containing content and metadata for publishing to a specific platform.
/// </summary>
public class Post
{
    /// <summary>
    /// The social media platform identifier (e.g., "Twitter", "Facebook", "LinkedIn").
    /// </summary>
    public required string Platform { get; set; }

    /// <summary>
    /// The text content of the post.
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// The optional URL to media content (image, video, etc.) to be included with the post.
    /// </summary>
    public string? MediaUrl { get; set; }

    /// <summary>
    /// The optional URL to be included as a link in the post.
    /// </summary>
    public string? LinkUrl { get; set; }
}
