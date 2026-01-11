namespace SocialManager.Data.Types.Social;


/// <summary>
/// Represents a collection of social media posts with associated categorization and tagging for blog content.
/// </summary>
public class PostCollection
{
    /// <summary>
    /// The category associated with the posts.
    /// </summary>
    public Category? Category { get; set; }
    
    /// <summary>
    /// The list of tags associated with the posts.
    /// </summary>
    public List<Tag>? Tags { get; set; }

    /// <summary>
    /// The list of social media posts in the collection.
    /// </summary>
    public List<Post> Posts { get; set; } = [];

    /// <summary>
    /// Attached non-structured metadata.
    /// </summary>
    public Dictionary<string, string> MetaData = [];
}
