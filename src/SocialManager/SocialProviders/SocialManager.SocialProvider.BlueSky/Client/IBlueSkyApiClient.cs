using SocialManager.SocialProvider.BlueSky.Client.Models;

namespace SocialManager.SocialProvider.BlueSky.Client;

/// <summary>
/// Abstraction for BlueSky AT Protocol client operations.
/// </summary>
public interface IBlueSkyApiClient
{
    /// <summary>
    /// Posts content to BlueSky with the specified text.
    /// </summary>
    /// <param name="text">The text content of the post.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created post response.</returns>
    Task<BlueSkyPostResponse> PublishPostAsync(string text, CancellationToken ct);

    /// <summary>
    /// Posts content to BlueSky with media attachment.
    /// </summary>
    /// <param name="text">The text content of the post.</param>
    /// <param name="mediaData">The media data as byte array.</param>
    /// <param name="mimeType">The MIME type of the media (e.g., "image/jpeg").</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created post response.</returns>
    Task<BlueSkyPostResponse> PublishPostWithMediaAsync(string text, byte[] mediaData, string mimeType, CancellationToken ct);

    /// <summary>
    /// Gets a post by its AT URI.
    /// </summary>
    /// <param name="postUri">The AT URI of the post (at://did:plc:xxx/app.bsky.feed.post/xxx).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The post response, or null if not found.</returns>
    Task<BlueSkyPostResponse?> GetPostAsync(string postUri, CancellationToken ct);

    /// <summary>
    /// Gets the authenticated user's information.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The authenticated user response.</returns>
    Task<BlueSkyAuthenticatedUserResponse> GetAuthenticatedUserAsync(CancellationToken ct);
}
