namespace SocialManager.Data.Types.Social;

/// <summary>
/// Represents engagement statistics for a social media post.
/// </summary>
public class Statistic
{
    /// <summary>
    /// The unique identifier of the post.
    /// </summary>
    public required string PostId { get; set; }
    
    /// <summary>
    /// The title of the post.
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// The social media platform where the post was published.
    /// </summary>
    public required string Platform { get; set; }

    /// <summary>
    /// The date and time at which the data was retrieved.
    /// </summary>
    public required DateTime RetrievedAt { get; set; } = DateTime.Now;
    
    /// <summary>
    /// The number of likes the post has received.
    /// </summary>
    public int? Likes { get; set; }
    
    /// <summary>
    /// The number of times the post has been shared.
    /// </summary>
    public int? Shares { get; set; }
    
    /// <summary>
    /// The number of comments the post has received.
    /// </summary>
    public int? Comments { get; set; }
}