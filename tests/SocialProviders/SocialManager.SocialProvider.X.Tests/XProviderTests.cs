using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SocialManager.Data.Types.Social;
using SocialManager.SocialProvider.X;
using SocialManager.SocialProvider.X.Client;
using SocialManager.SocialProvider.X.Client.Models;
using SocialProvider.Exceptions;

namespace SocialManager.SocialProvider.X.Tests;

public class XProviderTests
{
    private readonly Mock<IXApiClient> _mockClient;
    private readonly Mock<ILogger<XProvider>> _mockLogger;
    private readonly XProvider _provider;

    public XProviderTests()
    {
        _mockClient = new Mock<IXApiClient>();
        _mockLogger = new Mock<ILogger<XProvider>>();
        _provider = new XProvider(_mockClient.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithNullClient_ThrowsArgumentNullException()
    {
        var act = () => new XProvider(null!, _mockLogger.Object);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("client");
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        var act = () => new XProvider(_mockClient.Object, null!);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("logger");
    }

    [Fact]
    public async Task Post_WithValidTextPost_PublishesTweet()
    {
        var post = new Post
        {
            Platform = "X",
            Content = "Test tweet content",
            MediaUrl = null,
            LinkUrl = null
        };

        var mockTweetResponse = new XTweetResponse
        {
            Id = "123456789",
            Text = post.Content,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.PublishTweetAsync(post.Content, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockTweetResponse);

        await _provider.Post(post, CancellationToken.None);

        _mockClient.Verify(
            c => c.PublishTweetAsync(post.Content, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Post_WithLinkUrl_AppendsLinkToContent()
    {
        var post = new Post
        {
            Platform = "X",
            Content = "Check this out!",
            MediaUrl = null,
            LinkUrl = "https://example.com"
        };

        var expectedContent = $"{post.Content}\n\n{post.LinkUrl}";

        var mockTweetResponse = new XTweetResponse
        {
            Id = "123456789",
            Text = expectedContent,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.PublishTweetAsync(expectedContent, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockTweetResponse);

        await _provider.Post(post, CancellationToken.None);

        _mockClient.Verify(
            c => c.PublishTweetAsync(expectedContent, It.IsAny<CancellationToken>()),
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
            Platform = "X",
            Content = "",
            MediaUrl = null,
            LinkUrl = null
        };

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.Post(post, CancellationToken.None));

        exception.Message.Should().Contain("cannot be empty");
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public async Task Post_WithContentExceeding280Characters_ThrowsSocialProviderException()
    {
        var longContent = new string('x', 281);
        var post = new Post
        {
            Platform = "X",
            Content = longContent,
            MediaUrl = null,
            LinkUrl = null
        };

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.Post(post, CancellationToken.None));

        exception.Message.Should().Contain("exceeds maximum length");
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public async Task GetStatistics_WithValidTweetId_ReturnsStatistics()
    {
        var tweetId = "123456789";
        var mockTweetResponse = new XTweetResponse
        {
            Id = tweetId,
            Text = "Test tweet text",
            LikeCount = 42,
            RetweetCount = 15,
            ReplyCount = 7,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.GetTweetAsync(tweetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockTweetResponse);

        var result = await _provider.GetStatistics(tweetId, CancellationToken.None);

        result.Should().NotBeNull();
        result.PostId.Should().Be(tweetId);
        result.Platform.Should().Be("X");
        result.Likes.Should().Be(42);
        result.Shares.Should().Be(15);
        result.Comments.Should().Be(7);
        result.Title.Should().Be("Test tweet text");
    }

    [Fact]
    public async Task GetStatistics_WithLongTweetText_TruncatesTitle()
    {
        var tweetId = "123456789";
        var longText = new string('x', 100);
        var mockTweetResponse = new XTweetResponse
        {
            Id = tweetId,
            Text = longText,
            LikeCount = 10,
            CreatedAt = DateTime.UtcNow
        };

        _mockClient
            .Setup(c => c.GetTweetAsync(tweetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockTweetResponse);

        var result = await _provider.GetStatistics(tweetId, CancellationToken.None);

        result.Title.Length.Should().Be(53);
        result.Title.Should().EndWith("...");
    }

    [Fact]
    public async Task GetStatistics_WithNonExistentTweet_ThrowsSocialProviderException()
    {
        var tweetId = "999999999";

        _mockClient
            .Setup(c => c.GetTweetAsync(tweetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((XTweetResponse?)null);

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.GetStatistics(tweetId, CancellationToken.None));

        exception.Message.Should().Contain("not found");
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public async Task Post_WhenClientThrowsSocialProviderException_PropagatesException()
    {
        var post = new Post
        {
            Platform = "X",
            Content = "Test",
            MediaUrl = null,
            LinkUrl = null
        };

        var expectedException = new SocialProviderException("Test error", "X");

        _mockClient
            .Setup(c => c.PublishTweetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.Post(post, CancellationToken.None));

        exception.Should().Be(expectedException);
    }

    [Fact]
    public async Task GetStatistics_WhenClientThrowsSocialProviderException_PropagatesException()
    {
        var tweetId = "123456789";
        var expectedException = new SocialProviderException("Test error", "X");

        _mockClient
            .Setup(c => c.GetTweetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<SocialProviderException>(
            async () => await _provider.GetStatistics(tweetId, CancellationToken.None));

        exception.Should().Be(expectedException);
    }
}
