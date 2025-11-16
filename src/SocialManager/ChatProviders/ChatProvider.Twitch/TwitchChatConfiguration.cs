namespace ChatProvider.Twitch;

public class TwitchChatConfiguration
{
    /// <summary>
    /// The Twitch bot's username
    /// </summary>
    public required string BotUsername { get; init; }

    /// <summary>
    /// The OAuth access token for the bot (should start with "oauth:")
    /// </summary>
    public required string AccessToken { get; init; }

    /// <summary>
    /// The Twitch channel to connect to (without the # prefix)
    /// </summary>
    public required string Channel { get; init; }

    /// <summary>
    /// The Twitch channel ID (required for PubSub features like channel point redemptions)
    /// </summary>
    public string? ChannelId { get; init; }
}
