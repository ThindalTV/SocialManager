using SocialManager.Data.Types.Social;

namespace SocialManager.Data.Types;

/// <summary>
/// Represents a collection containing blog content and social media posts with associated metadata.
/// </summary>
public class Entry : BaseType
{
    /// <summary>
    /// The title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The blog post content and categorization.
    /// </summary>
    public PostCollection? BlogPost { get; set; }

    /// <summary>
    /// The list of social media posts.
    /// </summary>
    public List<Social.Post> SocialPosts { get; set; } = [];
}
