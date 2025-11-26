using LinqToTwitter;
using LinqToTwitter.Common;
using LinqToTwitter.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialManager.SocialProvider.X.Client.Models;
using SocialManager.SocialProvider.X.Configuration;
using SocialProvider.Exceptions;
using System.Net;

namespace SocialManager.SocialProvider.X.Client;

/// <summary>
/// Implementation of X (Twitter) API client using LinqToTwitter library.
/// </summary>
public class LinqToTwitterApiClient : IXApiClient
{
    private readonly TwitterContext _context;
    private readonly ILogger<LinqToTwitterApiClient> _logger;
    private readonly XProviderConfiguration _configuration;
    private const string PlatformName = "X";

    public LinqToTwitterApiClient(
        IOptions<XProviderConfiguration> configuration,
        ILogger<LinqToTwitterApiClient> logger)
    {
        _configuration = configuration.Value;
        _logger = logger;

        _configuration.Validate();

        var auth = new SingleUserAuthorizer
        {
            CredentialStore = new SingleUserInMemoryCredentialStore
            {
                ConsumerKey = _configuration.ApiKey,
                ConsumerSecret = _configuration.ApiSecret,
                AccessToken = _configuration.AccessToken,
                AccessTokenSecret = _configuration.AccessTokenSecret
            }
        };

        _context = new TwitterContext(auth);
    }

    public async Task<XTweetResponse> PublishTweetAsync(string text, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Publishing tweet with text length: {Length}", text.Length);

            var tweetResponse = await _context.TweetAsync(text);

            if (tweetResponse == null)
            {
                throw new SocialProviderException("Failed to publish tweet: No response from API", PlatformName);
            }

            _logger.LogInformation("Successfully published tweet with ID: {TweetId}", tweetResponse.ID);

            return new XTweetResponse
            {
                Id = tweetResponse.ID ?? string.Empty,
                Text = tweetResponse.Text ?? text,
                CreatedAt = DateTime.UtcNow
            };
        }
        catch (TwitterQueryException ex)
        {
            _logger.LogError(ex, "Twitter API error while publishing tweet");
            throw HandleTwitterException(ex);
        }
    }

    public async Task<XTweetResponse> PublishTweetWithMediaAsync(
        string text,
        byte[] mediaData,
        CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Uploading media ({Size} bytes) for tweet", mediaData.Length);

            var mediaResponse = await _context.UploadMediaAsync(
                media: mediaData,
                mediaType: "image/jpeg",
                mediaCategory: "tweet_image");

            if (mediaResponse == null || mediaResponse.MediaID == 0)
            {
                throw new SocialProviderException("Failed to upload media", PlatformName);
            }

            _logger.LogInformation("Media uploaded with ID: {MediaId}", mediaResponse.MediaID);

            // TODO: LinqToTwitter v6.15.0 TweetAsync signature needs verification for media IDs
            // For now, post text without media - this needs to be tested with real credentials
            _logger.LogWarning("Media upload completed but attaching to tweet needs LinqToTwitter API verification. Posting text only for now.");

            var tweetResponse = await _context.TweetMediaAsync(text, [mediaResponse.MediaID.ToString()], cancelToken: ct);

            if (tweetResponse == null)
            {
                throw new SocialProviderException("Failed to publish tweet with media: No response from API", PlatformName);
            }

            _logger.LogInformation("Successfully published tweet, ID: {TweetId}. Note: Media attachment pending API signature verification.", tweetResponse.ID);

            return new XTweetResponse
            {
                Id = tweetResponse.ID ?? string.Empty,
                Text = tweetResponse.Text ?? text,
                CreatedAt = DateTime.UtcNow
            };
        }
        catch (TwitterQueryException ex)
        {
            _logger.LogError(ex, "Twitter API error while publishing tweet with media");
            throw HandleTwitterException(ex);
        }
    }

    public async Task<XTweetResponse?> GetTweetAsync(string tweetId, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Retrieving tweet with ID: {TweetId}", tweetId);

            var tweetQuery = await _context.Tweets
                .Where(t => t.Type == TweetType.Lookup &&
                           t.Ids == tweetId &&
                           t.TweetFields == "created_at,public_metrics,author_id" &&
                           t.Expansions == "author_id" &&
                           t.UserFields == "username")
                .SingleOrDefaultAsync();

            if (tweetQuery?.Tweets == null || !tweetQuery.Tweets.Any())
            {
                _logger.LogWarning("Tweet not found: {TweetId}", tweetId);
                return null;
            }

            var tweetData = tweetQuery.Tweets[0];
            var author = tweetQuery.Includes?.Users?.FirstOrDefault();

            _logger.LogInformation("Successfully retrieved tweet: {TweetId}", tweetId);

            return new XTweetResponse
            {
                Id = tweetData.ID ?? tweetId,
                Text = tweetData.Text ?? string.Empty,
                LikeCount = tweetData.PublicMetrics?.LikeCount,
                RetweetCount = tweetData.PublicMetrics?.RetweetCount,
                ReplyCount = tweetData.PublicMetrics?.ReplyCount,
                AuthorUsername = author?.Username,
                CreatedAt = tweetData.CreatedAt
            };
        }
        catch (TwitterQueryException ex)
        {
            _logger.LogError(ex, "Twitter API error while retrieving tweet: {TweetId}", tweetId);
            throw HandleTwitterException(ex);
        }
    }

    public async Task<XAuthenticatedUserResponse> GetAuthenticatedUserAsync(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Retrieving authenticated user information");

            var accountResponse = await _context.Account
                .Where(account => account.Type == AccountType.VerifyCredentials)
                .SingleOrDefaultAsync();

            if (accountResponse?.User == null)
            {
                throw new SocialProviderAuthenticationException("Failed to retrieve authenticated user", PlatformName);
            }

            _logger.LogInformation("Successfully authenticated as: {ScreenName}", accountResponse.User.ScreenNameResponse);

            return new XAuthenticatedUserResponse
            {
                Id = accountResponse.User.UserIDResponse ?? string.Empty,
                Name = accountResponse.User.Name ?? string.Empty,
                Username = accountResponse.User.ScreenNameResponse ?? string.Empty
            };
        }
        catch (TwitterQueryException ex)
        {
            _logger.LogError(ex, "Twitter API error while retrieving authenticated user");
            throw HandleTwitterException(ex);
        }
    }

    private Exception HandleTwitterException(TwitterQueryException ex)
    {
        var statusCode = ex.StatusCode;

        if (statusCode == HttpStatusCode.TooManyRequests || (int)statusCode == 429)
        {
            var message = "Rate limit exceeded. Please wait before retrying.";
            return new SocialProviderRateLimitException(message, null, ex, PlatformName);
        }

        if (statusCode == HttpStatusCode.Unauthorized)
        {
            return new SocialProviderAuthenticationException(
                "Authentication failed. Please check your API credentials.", ex, PlatformName);
        }

        if (statusCode == HttpStatusCode.Forbidden)
        {
            return new SocialProviderException(
                "Access forbidden. Check API permissions and account status.", ex, PlatformName);
        }

        if (statusCode == HttpStatusCode.NotFound)
        {
            return new SocialProviderException("Resource not found.", ex, PlatformName);
        }

        return new SocialProviderException(
            $"X API error: {ex.Message}", ex, PlatformName);
    }
}
