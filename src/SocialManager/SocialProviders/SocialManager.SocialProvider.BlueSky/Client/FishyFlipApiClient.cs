using FishyFlip;
using FishyFlip.Lexicon;
using FishyFlip.Lexicon.App.Bsky.Embed;
using FishyFlip.Lexicon.App.Bsky.Feed;
using FishyFlip.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialManager.SocialProvider.BlueSky.Client.Models;
using SocialManager.SocialProvider.BlueSky.Configuration;
using SocialProvider.Exceptions;

namespace SocialManager.SocialProvider.BlueSky.Client;

/// <summary>
/// Implementation of BlueSky API client using FishyFlip library (v4.1.0).
/// </summary>
public class FishyFlipApiClient : IBlueSkyApiClient
{
    private readonly ATProtocol _protocol;
    private readonly BlueSkyProviderConfiguration _config;
    private readonly ILogger<FishyFlipApiClient> _logger;
    private const string PlatformName = "BlueSky";
    private bool _isAuthenticated;

    public FishyFlipApiClient(
        IOptions<BlueSkyProviderConfiguration> config,
        ILogger<FishyFlipApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(logger);

        _config = config.Value;
        _logger = logger;

        _config.Validate();

        var builder = new ATProtocolBuilder()
            .WithInstanceUrl(new Uri(_config.PdsUrl))
            .WithLogger(logger);

        _protocol = builder.Build();
    }

    /// <inheritdoc />
    public async Task<BlueSkyPostResponse> PublishPostAsync(string text, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        await EnsureAuthenticatedAsync(ct);

        _logger.LogInformation("Publishing post to BlueSky");

        try
        {
            // Create a post record using the app.bsky.feed.post lexicon
            var post = new Post
            {
                Text = text,
                CreatedAt = DateTime.UtcNow,
                // Langs can be set if needed, e.g., new[] { "en" }
            };

            var result = await _protocol.Feed.CreatePostAsync(post, cancellationToken: ct);

            if (result?.AsT0?.Uri == null || result?.AsT0?.Cid == null)
            {
                throw new SocialProviderException("Failed to create post: No URI returned", PlatformName);
            }

            var response = new BlueSkyPostResponse
            {
                Uri = result.AsT0.Uri.ToString(),
                Cid = result.AsT0.Cid.ToString(),
                // Additional fields can be populated if available from result
            };

            _logger.LogInformation("Post published successfully: {Uri}", response.Uri);
            return response;
        }
        catch (Exception ex) when (ex is not SocialProviderException)
        {
            _logger.LogError(ex, "Failed to publish post to BlueSky");
            throw new SocialProviderException("Failed to publish post to BlueSky", ex, PlatformName);
        }
    }

    /// <inheritdoc />
    public async Task<BlueSkyPostResponse> PublishPostWithMediaAsync(string text, byte[] mediaData, string mimeType, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentNullException.ThrowIfNull(mediaData);
        ArgumentException.ThrowIfNullOrWhiteSpace(mimeType);

        await EnsureAuthenticatedAsync(ct);

        _logger.LogInformation("Uploading media to BlueSky, size: {Size} bytes", mediaData.Length);

        try
        {
            // Step 1: Upload the blob (media file)
            using var stream = new MemoryStream(mediaData);
            // cast the stream to StreamContent
            var streamContent = new StreamContent(stream);
            var blobResult = await _protocol.Repo.UploadBlobAsync(streamContent, ct);

            if (blobResult?.AsT0?.Blob == null)
            {
                throw new SocialProviderException("Failed to upload media blob", PlatformName);
            }

            _logger.LogInformation("Media uploaded successfully, creating post with image");

            // Step 2: Create an image embed with the uploaded blob
            var image = new Image(blobResult.AsT0.Blob, alt: text);

            // Create an ATObject of images
            var images = new EmbedImages([image]);

            // Step 3: Create the post with the image embed
            var post = new Post
            {
                Text = text,
                CreatedAt = DateTime.UtcNow,
                Embed = images
            };

            var result = await _protocol.Feed.CreatePostAsync(post, cancellationToken: ct);

            if (result?.AsT0?.Uri == null || result?.AsT0?.Cid == null)
            {
                throw new SocialProviderException("Failed to create post: No URI returned", PlatformName);
            }

            var response = new BlueSkyPostResponse
            {
                Uri = result.AsT0.Uri.ToString(),
                Cid = result.AsT0.Cid.ToString(),
            };

            _logger.LogInformation("Post with media published successfully: {Uri}", response.Uri);
            return response;
        }
        catch (Exception ex) when (ex is not SocialProviderException)
        {
            _logger.LogError(ex, "Failed to publish post with media to BlueSky");
            throw new SocialProviderException("Failed to publish post with media to BlueSky", ex, PlatformName);
        }
    }

    /// <inheritdoc />
    public async Task<BlueSkyPostResponse?> GetPostAsync(string postUri, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(postUri);

        await EnsureAuthenticatedAsync(ct);

        _logger.LogInformation("Retrieving post from BlueSky: {Uri}", postUri);
        
        try
        {
            var result = await _protocol.Feed.GetPostAsync(postUri, cancellationToken: ct);
            
            if (result?.AsT0 == null)
            {
                _logger.LogWarning("Post {Uri} not found - API returned null", postUri);
                return null;
            }

            var threadViewPost = result.AsT0.Value as FishyFlip.Lexicon.App.Bsky.Feed.ThreadViewPost;
            
            // The post data is in the Thread property
            if (threadViewPost?.Post == null)
            {
                _logger.LogWarning("Post {Uri} has no thread data", postUri);
                return null;
            }

            var postView = threadViewPost.Post;

            // Extract the Post record from the postView
            var postRecord = postView.Record as Post;

            var response = new BlueSkyPostResponse
            {
                Uri = postView.Uri?.ToString() ?? postUri,
                Cid = postView.Cid?.ToString() ?? string.Empty,
                Text = postRecord?.Text,
                LikeCount = (int)(postView.LikeCount ?? 0),
                RepostCount = (int)(postView.RepostCount ?? 0),
                ReplyCount = (int)(postView.ReplyCount ?? 0),
                QuoteCount = (int)(postView.QuoteCount ?? 0),
                CreatedAt = postRecord?.CreatedAt ?? DateTime.UtcNow,
                AuthorDid = postView.Author?.Did?.ToString(),
                AuthorHandle = postView.Author?.Handle?.Handle,
            };

            _logger.LogInformation("Successfully retrieved post {Uri} by {Author}", response.Uri, response.AuthorHandle);
            return response;
        }
        catch (Exception ex) when (ex is not SocialProviderException)
        {
            _logger.LogError(ex, "Failed to retrieve post from BlueSky: {Uri}", postUri);
            throw new SocialProviderException("Failed to retrieve post from BlueSky", ex, PlatformName);
        }
    }

    /// <inheritdoc />
    public async Task<BlueSkyAuthenticatedUserResponse> GetAuthenticatedUserAsync(CancellationToken ct)
    {
        await EnsureAuthenticatedAsync(ct);

        _logger.LogInformation("Retrieving authenticated user profile");

        if (_protocol?.Session?.Did == null)
        {
            throw new SocialProviderException("No active session found", PlatformName);
        }

        var at = ATDid.Create(_protocol.Session.Did.ToString())
            ?? throw new SocialProviderException("Failed to create ATDid from session Did", PlatformName);

        var profile = (await _protocol.Actor.GetProfileAsync(at, ct))?.AsT0
            ?? throw new SocialProviderException("Failed to retrieve user profile", PlatformName);

        var ret = new BlueSkyAuthenticatedUserResponse()
        {
            Did = profile.Did.ToString(),
            Avatar = profile.Avatar,
            Description = profile.Description,
            DisplayName = profile.DisplayName,
            Handle = profile.Handle.Handle,
            FollowersCount = (int)(profile.FollowersCount ?? 0),
            FollowingCount = (int)(profile.FollowsCount ?? 0),
            PostsCount = (int)(profile.PostsCount ?? 0)
        };

        return ret;
    }

    private async Task EnsureAuthenticatedAsync(CancellationToken ct)
    {
        if (_isAuthenticated && _protocol.Session != null)
        {
            return;
        }

        try
        {
            _logger.LogInformation("Authenticating with BlueSky using identifier: {Identifier}", _config.Identifier);

            var result = await _protocol.Server.CreateSessionAsync(_config.Identifier, _config.AppPassword, cancellationToken: ct);
            
            if (result?.AsT0 == null)
            {
                throw new SocialProviderAuthenticationException(
                    "Failed to create session with BlueSky - invalid credentials or server error",
                    PlatformName);
            }

            _isAuthenticated = true;
            _logger.LogInformation("Successfully authenticated with BlueSky as {Handle}", result.AsT0.Handle);
        }
        catch (SocialProviderAuthenticationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during authentication");
            throw new SocialProviderAuthenticationException(
                "Failed to authenticate with BlueSky",
                ex,
                PlatformName);
        }
    }
}
