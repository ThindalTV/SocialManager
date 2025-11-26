using SocialManager.SocialProvider.X.Client.Models;

namespace SocialManager.SocialProvider.X.Client;

/// <summary>
/// Abstraction for X (Twitter) API client operations.
/// </summary>
public interface IXApiClient
{
    /// <summary>
    /// Posts a tweet with the specified content.
    /// </summary>
    Task<XTweetResponse> PublishTweetAsync(string text, CancellationToken ct);

    /// <summary>
    /// Posts a tweet with media attachment.
    /// </summary>
    Task<XTweetResponse> PublishTweetWithMediaAsync(string text, byte[] mediaData, CancellationToken ct);

    /// <summary>
    /// Gets a tweet by its ID.
    /// </summary>
    Task<XTweetResponse?> GetTweetAsync(string tweetId, CancellationToken ct);

    /// <summary>
    /// Gets the authenticated user's information.
    /// </summary>
    Task<XAuthenticatedUserResponse> GetAuthenticatedUserAsync(CancellationToken ct);
}
