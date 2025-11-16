using ChatProvider.Twitch;
using ChatProvider.Types;

namespace SocialManager.Tests.ChatProviders;

public class TwitchChatProviderTests
{
    [Fact]
    public void Constructor_ShouldInitializeProvider()
    {
        // Arrange & Act
        var provider = new TwitchChatProvider();

        // Assert
        Assert.NotNull(provider);
        Assert.Equal("Twitch", provider.ChatPlatform);
    }

    [Fact]
    public void Configure_WithValidConfiguration_ShouldSucceed()
    {
        // Arrange
        var provider = new TwitchChatProvider();
        var config = new TwitchChatConfiguration
        {
            BotUsername = "testbot",
            AccessToken = "oauth:test_token",
            Channel = "testchannel"
        };

        // Act & Assert (no exception should be thrown)
        provider.Configure(config);
    }

    [Fact]
    public void Configure_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        // Arrange
        var provider = new TwitchChatProvider();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => provider.Configure(null!));
    }

    [Fact]
    public async Task Connect_WithoutConfiguration_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var provider = new TwitchChatProvider();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => provider.Connect(CancellationToken.None));
    }

    [Fact]
    public async Task SendMessageAsync_WithoutConnection_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var provider = new TwitchChatProvider();
        var config = new TwitchChatConfiguration
        {
            BotUsername = "testbot",
            AccessToken = "oauth:test_token",
            Channel = "testchannel"
        };
        provider.Configure(config);

        var message = new Message
        {
            ChatPlatform = "Twitch",
            Sender = "testbot",
            TextMessage = new TextMessage
            {
                Content = "Test message",
                ContentHtml = "Test message"
            },
            Direction = ChatDirection.Sending,
            Timestamp = DateTime.UtcNow
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => provider.SendMessageAsync(message, CancellationToken.None));
    }

    [Fact]
    public async Task Disconnect_WithoutConnection_ShouldNotThrow()
    {
        // Arrange
        var provider = new TwitchChatProvider();

        // Act & Assert (should not throw)
        await provider.Disconnect(CancellationToken.None);
    }
}
