namespace SocialManager.SocialProvider.BlueSky.Client.Models;

/// <summary>
/// Response model for authenticated BlueSky user information.
/// </summary>
public class BlueSkyAuthenticatedUserResponse
{
    /// <summary>
    /// Gets or sets the user's DID (Decentralized Identifier).
    /// </summary>
    public string Did { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's handle (e.g., user.bsky.social).
    /// </summary>
    public string Handle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the user's profile description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the user's avatar URL.
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// Gets or sets the number of followers.
    /// </summary>
    public int FollowersCount { get; set; }

    /// <summary>
    /// Gets or sets the number of users being followed.
    /// </summary>
    public int FollowingCount { get; set; }

    /// <summary>
    /// Gets or sets the number of posts.
    /// </summary>
    public int PostsCount { get; set; }
}
