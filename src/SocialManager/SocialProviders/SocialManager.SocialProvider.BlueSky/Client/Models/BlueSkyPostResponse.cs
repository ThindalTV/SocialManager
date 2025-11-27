namespace SocialManager.SocialProvider.BlueSky.Client.Models;

/// <summary>
/// Response model for BlueSky post data.
/// </summary>
public class BlueSkyPostResponse
{
    /// <summary>
    /// Gets or sets the AT URI of the post (at://did:plc:xxx/app.bsky.feed.post/xxx).
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CID (Content Identifier) of the post.
    /// </summary>
    public string Cid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text content of the post.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the number of likes on the post.
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// Gets or sets the number of reposts of the post.
    /// </summary>
    public int RepostCount { get; set; }

    /// <summary>
    /// Gets or sets the number of replies to the post.
    /// </summary>
    public int ReplyCount { get; set; }

    /// <summary>
    /// Gets or sets the number of quote posts of the post.
    /// </summary>
    public int QuoteCount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the post was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the author's DID (Decentralized Identifier).
    /// </summary>
    public string? AuthorDid { get; set; }

    /// <summary>
    /// Gets or sets the author's handle.
    /// </summary>
    public string? AuthorHandle { get; set; }
}
