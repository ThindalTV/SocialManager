using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SocialManager.Data.Types.Social;
using SocialManager.SocialProvider.BlueSky;
using SocialManager.SocialProvider.BlueSky.Client;
using SocialManager.SocialProvider.BlueSky.Client.Models;
using SocialProvider.Exceptions;

namespace SocialManager.SocialProvider.BlueSky.Tests;

public class BlueSkyProviderTests
{
    private readonly Mock<IBlueSkyApiClient> _mockClient;
    private readonly Mock<ILogger<BlueSkyProvider>> _mockLogger;
    private readonly BlueSkyProvider _provider;

    public BlueSkyProviderTests()
    {
        _mockClient = new Mock<IBlueSkyApiClient>();
        _mockLogger = new Mock<ILogger<BlueSkyProvider>>();
        _provider = new BlueSkyProvider(_mockClient.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullClient_ThrowsArgumentNullException()
    {
        var act = () => new BlueSkyProvider(null!, _mockLogger.Object);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("client");
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        var act = () => new BlueSkyProvider(_mockClient.Object, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    [Fact]
    public async Task Post_WithValidTextPost_PublishesPost()
    {
        var post = new Post
        {
            Platform = "BlueSky",
            Content = "Test post content",
            MediaUrl = null,
            LinkUrl = null
        };

        var mockPostResponse = new BlueSkyPostResponse
        {
            Uri = "at://did:plc:test/app.bsky.feed.post/test123",
            Cid = "test-cid",
            Text = post.Content,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.PublishPostAsync(post.Content, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockPostResponse);

        await _provider.Post(post, CancellationToken.None);

        _mockClient.Verify(
            c => c.PublishPostAsync(post.Content, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Post_WithLinkUrl_AppendsLinkToContent()
    {
        var post = new Post
        {
            Platform = "BlueSky",
            Content = "Check this out!",
            MediaUrl = null,
            LinkUrl = "https://example.com"
        };

        var expectedContent = $"{post.Content}\n\n{post.LinkUrl}";

        var mockPostResponse = new BlueSkyPostResponse
        {
            Uri = "at://did:plc:test/app.bsky.feed.post/test123",
            Cid = "test-cid",
            Text = expectedContent,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.PublishPostAsync(expectedContent, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockPostResponse);

        await _provider.Post(post, CancellationToken.None);

        _mockClient.Verify(
            c => c.PublishPostAsync(expectedContent, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Post_WithNullPost_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await _provider.Post(null!, CancellationToken.None));
    }

    [Fact]
    public async Task Post_WithEmptyContent_ThrowsSocialProviderException()
    {
        var post = new Post
        {
            Platform = "BlueSky",
            Content = "",
            MediaUrl = null,
            LinkUrl = null
        };

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.Post(post, CancellationToken.None));

        exception.Message.Should().Contain("cannot be empty");
        exception.Platform.Should().Be("BlueSky");
    }

    [Fact]
    public async Task Post_WithContentExceeding300Characters_ThrowsSocialProviderException()
    {
        var longContent = new string('x', 301);
        var post = new Post
        {
            Platform = "BlueSky",
            Content = longContent,
            MediaUrl = null,
            LinkUrl = null
        };

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.Post(post, CancellationToken.None));

        exception.Message.Should().Contain("exceeds maximum length");
        exception.Platform.Should().Be("BlueSky");
    }

    [Fact]
    public async Task GetStatistics_WithValidPostUri_ReturnsStatistics()
    {
        var postUri = "at://did:plc:test/app.bsky.feed.post/test123";
        var mockPostResponse = new BlueSkyPostResponse
        {
            Uri = postUri,
            Cid = "test-cid",
            Text = "Test post text",
            LikeCount = 42,
            RepostCount = 10,
            QuoteCount = 5,
            ReplyCount = 7,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.GetPostAsync(postUri, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockPostResponse);

        var result = await _provider.GetStatistics(postUri, CancellationToken.None);

        result.Should().NotBeNull();
        result.PostId.Should().Be(postUri);
        result.Platform.Should().Be("BlueSky");
        result.Likes.Should().Be(42);
        result.Shares.Should().Be(15); // RepostCount + QuoteCount
        result.Comments.Should().Be(7);
        result.Title.Should().Be("Test post text");
    }

    [Fact]
    public async Task GetStatistics_WithLongPostText_TruncatesTitle()
    {
        var postUri = "at://did:plc:test/app.bsky.feed.post/test123";
        var longText = new string('x', 100);
        var mockPostResponse = new BlueSkyPostResponse
        {
            Uri = postUri,
            Cid = "test-cid",
            Text = longText,
            LikeCount = 10,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.GetPostAsync(postUri, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockPostResponse);

        var result = await _provider.GetStatistics(postUri, CancellationToken.None);

        result.Title.Length.Should().Be(53);
        result.Title.Should().EndWith("...");
    }

    [Fact]
    public async Task GetStatistics_WithNonExistentPost_ThrowsSocialProviderException()
    {
        var postUri = "at://did:plc:test/app.bsky.feed.post/nonexistent";

        _mockClient
            .Setup(c => c.GetPostAsync(postUri, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BlueSkyPostResponse?)null);

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.GetStatistics(postUri, CancellationToken.None));

        exception.Message.Should().Contain("not found");
        exception.Platform.Should().Be("BlueSky");
    }

    [Fact]
    public async Task Post_WhenClientThrowsSocialProviderException_PropagatesException()
    {
        var post = new Post
        {
            Platform = "BlueSky",
            Content = "Test",
            MediaUrl = null,
            LinkUrl = null
        };

        var expectedException = new SocialProviderException("Test error", "BlueSky");

        _mockClient
            .Setup(c => c.PublishPostAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.Post(post, CancellationToken.None));

        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task GetStatistics_WhenClientThrowsSocialProviderException_PropagatesException()
    {
        var postUri = "at://did:plc:test/app.bsky.feed.post/test123";
        var expectedException = new SocialProviderException("Test error", "BlueSky");

        _mockClient
            .Setup(c => c.GetPostAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.GetStatistics(postUri, CancellationToken.None));

        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task GetStatistics_WithNullOrWhiteSpacePostId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _provider.GetStatistics("", CancellationToken.None));

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _provider.GetStatistics("   ", CancellationToken.None));
    }

    [Fact]
    public async Task Post_WithContentAndLinkExceeding300Characters_ThrowsSocialProviderException()
    {
        var content = new string('x', 250);
        var link = "https://example.com/very/long/url/path/that/makes/total/exceed/limit";
        
        var post = new Post
        {
            Platform = "BlueSky",
            Content = content,
            MediaUrl = null,
            LinkUrl = link
        };

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.Post(post, CancellationToken.None));

        exception.Message.Should().Contain("exceeds maximum length");
        exception.Platform.Should().Be("BlueSky");
    }
}
