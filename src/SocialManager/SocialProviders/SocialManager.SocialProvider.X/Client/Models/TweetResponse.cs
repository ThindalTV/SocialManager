namespace SocialManager.SocialProvider.X.Client.Models;

/// <summary>
/// Represents a tweet response from the X API.
/// </summary>
public class XTweetResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the tweet.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the text content of the tweet.
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Gets or sets the number of likes (favorites) the tweet has received.
    /// </summary>
    public int? LikeCount { get; set; }

    /// <summary>
    /// Gets or sets the number of times the tweet has been retweeted.
    /// </summary>
    public int? RetweetCount { get; set; }

    /// <summary>
    /// Gets or sets the number of replies the tweet has received.
    /// </summary>
    public int? ReplyCount { get; set; }

    /// <summary>
    /// Gets or sets the author's username.
    /// </summary>
    public string? AuthorUsername { get; set; }

    /// <summary>
    /// Gets or sets when the tweet was created.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
}
