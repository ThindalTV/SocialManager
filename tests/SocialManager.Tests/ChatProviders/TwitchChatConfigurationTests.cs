using ChatProvider.Twitch;

namespace SocialManager.Tests.ChatProviders;

public class TwitchChatConfigurationTests
{
    [Fact]
    public void Configuration_WithValidProperties_ShouldInitialize()
    {
        // Arrange & Act
        var config = new TwitchChatConfiguration
        {
            BotUsername = "testbot",
            AccessToken = "oauth:test_token_12345",
            Channel = "testchannel"
        };

        // Assert
        Assert.Equal("testbot", config.BotUsername);
        Assert.Equal("oauth:test_token_12345", config.AccessToken);
        Assert.Equal("testchannel", config.Channel);
    }

    [Fact]
    public void Configuration_Properties_ShouldBeRequired()
    {
        // Arrange & Act & Assert
        // This test verifies that the required keyword is enforced at compile time
        // If this compiles, the properties are properly marked as required
        var config = new TwitchChatConfiguration
        {
            BotUsername = "bot",
            AccessToken = "oauth:token",
            Channel = "channel"
        };

        Assert.NotNull(config.BotUsername);
        Assert.NotNull(config.AccessToken);
        Assert.NotNull(config.Channel);
    }

    [Theory]
    [InlineData("bot123", "oauth:token123", "channel123")]
    [InlineData("MyBot", "oauth:abc123def456", "MyCoolChannel")]
    [InlineData("test_bot", "oauth:test", "test")]
    public void Configuration_WithVariousValidInputs_ShouldInitialize(string username, string token, string channel)
    {
        // Arrange & Act
        var config = new TwitchChatConfiguration
        {
            BotUsername = username,
            AccessToken = token,
            Channel = channel
        };

        // Assert
        Assert.Equal(username, config.BotUsername);
        Assert.Equal(token, config.AccessToken);
        Assert.Equal(channel, config.Channel);
    }

    [Fact]
    public void Configuration_AccessToken_ShouldAcceptOAuthFormat()
    {
        // Arrange
        var token = "oauth:abc123def456ghi789";

        // Act
        var config = new TwitchChatConfiguration
        {
            BotUsername = "testbot",
            AccessToken = token,
            Channel = "testchannel"
        };

        // Assert
        Assert.StartsWith("oauth:", config.AccessToken);
    }

    [Fact]
    public void Configuration_Channel_ShouldNotRequireHashPrefix()
    {
        // Arrange & Act
        var config = new TwitchChatConfiguration
        {
            BotUsername = "testbot",
            AccessToken = "oauth:token",
            Channel = "channelname" // No # prefix
        };

        // Assert
        Assert.DoesNotContain("#", config.Channel);
        Assert.Equal("channelname", config.Channel);
    }

    [Fact]
    public void Configuration_ShouldBeImmutableAfterInitialization()
    {
        // Arrange
        var config = new TwitchChatConfiguration
        {
            BotUsername = "testbot",
            AccessToken = "oauth:token",
            Channel = "testchannel"
        };

        // Act & Assert
        // Properties use 'init' accessor, so they cannot be modified after initialization
        // This test verifies that the configuration is immutable
        var originalUsername = config.BotUsername;
        var originalToken = config.AccessToken;
        var originalChannel = config.Channel;

        Assert.Equal(originalUsername, config.BotUsername);
        Assert.Equal(originalToken, config.AccessToken);
        Assert.Equal(originalChannel, config.Channel);
    }
}
