using Microsoft.Extensions.Logging;
using SocialManager.Data.Types.Social;
using SocialManager.SocialProvider.X.Client;
using SocialProvider;
using SocialProvider.Exceptions;

namespace SocialManager.SocialProvider.X;

/// <summary>
/// X (Twitter) platform social provider implementation.
/// </summary>
public class XProvider : ISocialProvider
{
    private readonly IXApiClient _client;
    private readonly ILogger<XProvider> _logger;
    private const int MaxTweetLength = 280;
    private const string PlatformName = "X";

    public XProvider(
        IXApiClient client,
        ILogger<XProvider> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task Post(Post post, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(post);

        _logger.LogInformation("Starting post to X platform");

        try
        {
            ValidatePost(post);

            var content = BuildTweetContent(post);

            if (!string.IsNullOrEmpty(post.MediaUrl))
            {
                await PostWithMedia(content, post.MediaUrl, ct);
            }
            else
            {
                await _client.PublishTweetAsync(content, ct);
            }

            _logger.LogInformation("Successfully posted to X platform");
        }
        catch (SocialProviderException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while posting to X");
            throw new SocialProviderException("Failed to post to X platform", ex, PlatformName);
        }
    }

    /// <inheritdoc />
    public async Task<Statistic> GetStatistics(string postId, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(postId);

        _logger.LogInformation("Retrieving statistics for post: {PostId}", postId);

        try
        {
            var tweet = await _client.GetTweetAsync(postId, ct);

            if (tweet == null)
            {
                throw new SocialProviderException($"Tweet not found: {postId}", PlatformName);
            }

            var statistic = new Statistic
            {
                PostId = postId,
                Title = tweet.Text?.Length > 50 
                    ? tweet.Text[..50] + "..." 
                    : tweet.Text ?? "Untitled",
                Platform = PlatformName,
                RetrievedAt = DateTime.UtcNow,
                Likes = tweet.LikeCount,
                Shares = tweet.RetweetCount,
                Comments = tweet.ReplyCount
            };

            _logger.LogInformation(
                "Retrieved statistics for tweet {PostId}: Likes={Likes}, Retweets={Retweets}, Replies={Replies}",
                postId, statistic.Likes, statistic.Shares, statistic.Comments);

            return statistic;
        }
        catch (SocialProviderException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving statistics from X");
            throw new SocialProviderException("Failed to retrieve statistics from X platform", ex, PlatformName);
        }
    }

    private void ValidatePost(Post post)
    {
        if (string.IsNullOrWhiteSpace(post.Content))
        {
            throw new SocialProviderException("Post content cannot be empty", PlatformName);
        }

        var contentLength = BuildTweetContent(post).Length;

        if (contentLength > MaxTweetLength)
        {
            throw new SocialProviderException(
                $"Tweet content exceeds maximum length of {MaxTweetLength} characters (current: {contentLength})", PlatformName);
        }
    }

    private string BuildTweetContent(Post post)
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

        if (mediaUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            mediaUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            using var httpClient = new HttpClient();
            mediaData = await httpClient.GetByteArrayAsync(mediaUrl, ct);
        }
        else if (File.Exists(mediaUrl))
        {
            mediaData = await File.ReadAllBytesAsync(mediaUrl, ct);
        }
        else
        {
            throw new SocialProviderException($"Invalid media URL or file path: {mediaUrl}", PlatformName);
        }

        if (mediaData.Length == 0)
        {
            throw new SocialProviderException("Media file is empty", PlatformName);
        }

        _logger.LogInformation("Media downloaded successfully, size: {Size} bytes", mediaData.Length);

        await _client.PublishTweetWithMediaAsync(content, mediaData, ct);
    }
}
