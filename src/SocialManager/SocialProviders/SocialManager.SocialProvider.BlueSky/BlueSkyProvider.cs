using Microsoft.Extensions.Logging;
using SocialManager.Data.Types.Social;
using SocialManager.SocialProvider.BlueSky.Client;
using SocialProvider;
using SocialProvider.Exceptions;

namespace SocialManager.SocialProvider.BlueSky;

/// <summary>
/// BlueSky platform social provider implementation.
/// </summary>
public class BlueSkyProvider : ISocialProvider
{
    private readonly IBlueSkyApiClient _client;
    private readonly ILogger<BlueSkyProvider> _logger;
    private const int MaxPostLength = 300;
    private const string PlatformName = "BlueSky";

    public BlueSkyProvider(
        IBlueSkyApiClient client,
        ILogger<BlueSkyProvider> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task Post(Post post, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(post);

        _logger.LogInformation("Starting post to BlueSky platform");

        try
        {
            ValidatePost(post);

            var content = BuildPostContent(post);

            if (!string.IsNullOrEmpty(post.MediaUrl))
            {
                await PostWithMedia(content, post.MediaUrl, ct);
            }
            else
            {
                await _client.PublishPostAsync(content, ct);
            }

            _logger.LogInformation("Successfully posted to BlueSky platform");
        }
        catch (SocialProviderException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while posting to BlueSky");
            throw new SocialProviderException("Failed to post to BlueSky platform", ex, PlatformName);
        }
    }

    /// <inheritdoc />
    public async Task<Statistic> GetStatistics(string postId, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(postId);

        _logger.LogInformation("Retrieving statistics for post: {PostId}", postId);

        try
        {
            var post = await _client.GetPostAsync(postId, ct);

            if (post == null)
            {
                throw new SocialProviderException($"Post not found: {postId}", PlatformName);
            }

            var statistic = new Statistic
            {
                PostId = postId,
                Title = post.Text?.Length > 50 
                    ? post.Text[..50] + "..." 
                    : post.Text ?? "Untitled",
                Platform = PlatformName,
                RetrievedAt = DateTime.UtcNow,
                Likes = post.LikeCount,
                Shares = post.RepostCount + post.QuoteCount,
                Comments = post.ReplyCount
            };

            _logger.LogInformation(
                "Retrieved statistics for post {PostId}: Likes={Likes}, Reposts={Reposts}, Replies={Replies}",
                postId, statistic.Likes, statistic.Shares, statistic.Comments);

            return statistic;
        }
        catch (SocialProviderException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving statistics from BlueSky");
            throw new SocialProviderException("Failed to retrieve statistics from BlueSky platform", ex, PlatformName);
        }
    }

    private void ValidatePost(Post post)
    {
        if (string.IsNullOrWhiteSpace(post.Content))
        {
            throw new SocialProviderException("Post content cannot be empty", PlatformName);
        }

        var contentLength = BuildPostContent(post).Length;

        if (contentLength > MaxPostLength)
        {
            throw new SocialProviderException(
                $"Post content exceeds maximum length of {MaxPostLength} characters (current: {contentLength})", PlatformName);
        }
    }

    private string BuildPostContent(Post post)
    {
        var content = post.Content;

        if (!string.IsNullOrEmpty(post.LinkUrl) && !content.Contains(post.LinkUrl))
        {
            content = $"{content}\n\n{post.LinkUrl}";
        }

        return content;
    }

    private async Task PostWithMedia(string content, string mediaUrl, CancellationToken ct)
    {
        _logger.LogInformation("Downloading media from: {MediaUrl}", mediaUrl);

        byte[] mediaData;
        string mimeType;

        if (mediaUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            mediaUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(mediaUrl, ct);
            response.EnsureSuccessStatusCode();
            
            mediaData = await response.Content.ReadAsByteArrayAsync(ct);
            mimeType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
        }
        else if (File.Exists(mediaUrl))
        {
            mediaData = await File.ReadAllBytesAsync(mediaUrl, ct);
            mimeType = GetMimeTypeFromExtension(Path.GetExtension(mediaUrl));
        }
        else
        {
            throw new SocialProviderException($"Invalid media URL or file path: {mediaUrl}", PlatformName);
        }

        if (mediaData.Length == 0)
        {
            throw new SocialProviderException("Media file is empty", PlatformName);
        }

        _logger.LogInformation("Media downloaded successfully, size: {Size} bytes, type: {Type}", mediaData.Length, mimeType);

        await _client.PublishPostWithMediaAsync(content, mediaData, mimeType, ct);
    }

    private static string GetMimeTypeFromExtension(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };
    }
}
