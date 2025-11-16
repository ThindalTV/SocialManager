# ChatProvider.Twitch

A Twitch chat integration provider implementing the `IChatProvider` interface using TwitchLib.

## Features

? Connect to Twitch chat channels  
? Receive messages with full formatting  
? Send messages to chat  
? HTML-formatted output with badges, emotes, and mentions  
? Configurable connection settings  
? Event-driven architecture  

## Installation

Add the package reference to your project:

```xml
<ItemGroup>
  <PackageReference Include="TwitchLib.Client" Version="3.4.0" />
</ItemGroup>
```

## Quick Start

```csharp
using ChatProvider.Twitch;

// 1. Create and configure the provider
var config = new TwitchChatConfiguration 
{
    BotUsername = "your_bot_username",
    AccessToken = "oauth:your_oauth_token",
    Channel = "channel_to_join"
};

var provider = new TwitchChatProvider();
provider.Configure(config);

// 2. Set up message handler
provider.OnChatRecieved = (message) => {
    Console.WriteLine($"[{message.Sender}] {message.Content}");
    // Use message.ContentHtml for formatted output
};

// 3. Connect
await provider.Connect(CancellationToken.None);

// 4. Send a message
await provider.SendMessageAsync(new ChatMessage
{
    ChatPlatform = "Twitch",
    Sender = "your_bot_username",
    Content = "Hello chat!",
    ContentHtml = "Hello chat!",
    Direction = ChatDirection.Sending,
    Timestamp = DateTime.UtcNow
}, CancellationToken.None);

// 5. Disconnect when done
await provider.Disconnect(CancellationToken.None);
```

## Configuration

### Getting Twitch OAuth Token

1. Go to https://twitchtokengenerator.com/
2. Select the required scopes (chat:read, chat:edit)
3. Copy the OAuth token (it will start with "oauth:")
4. Use this token in your configuration

### Configuration Options

```csharp
public class TwitchChatConfiguration
{
    public string BotUsername { get; init; }  // Your bot's Twitch username
    public string AccessToken { get; init; }  // OAuth token (starts with "oauth:")
    public string Channel { get; init; }      // Channel name (without #)
}
```

## HTML Formatting

The provider generates rich HTML output for messages. See [HTML_FORMATTING.md](HTML_FORMATTING.md) for details.

### Message Properties

Each received message includes:
- `Content` - Plain text message
- `ContentHtml` - HTML formatted message with badges, emotes, and mentions
- `Sender` - Display name of the sender
- `ChatPlatform` - Always "Twitch"
- `Direction` - `ChatDirection.Recieved` for incoming messages
- `Timestamp` - UTC timestamp of when the message was received

### HTML Features

#### Badges
```html
<span class="twitch-badges">
    <span class="twitch-badge twitch-badge-moderator" data-version="1" title="moderator"></span>
</span>
```

#### Emotes
```html
<img src="https://static-cdn.jtvnw.net/emoticons/v2/25/default/dark/1.0" 
     alt="Kappa" 
     title="Kappa" 
     class="twitch-emote" />
```

#### Mentions
```html
<span class="twitch-mention">@username</span>
```

### Using HTML in Blazor

```razor
@foreach (var msg in messages)
{
    <div class="chat-message">
        <span class="sender">@msg.Sender:</span>
        @((MarkupString)msg.ContentHtml)
    </div>
}

<link rel="stylesheet" href="TwitchChatStyles.css" />
```

## Rate Limiting

The provider is configured with Twitch's rate limits:
- 750 messages per 30 seconds
- Automatic throttling to prevent rate limit violations

## Error Handling

The provider handles common errors:
- Connection failures
- Disconnections
- Authentication errors

Events are available for monitoring:
- `OnConnected` - Fired when connection is established
- `OnDisconnected` - Fired when connection is lost
- `OnError` - Fired when an error occurs

## Architecture

```
TwitchChatProvider
??? TwitchClient (TwitchLib)
??? Configuration Management
??? Event Handling
?   ??? OnMessageReceived
?   ??? OnConnected
?   ??? OnDisconnected
?   ??? OnError
??? HTML Formatting
    ??? Badge Rendering
    ??? Emote Replacement
    ??? Mention Highlighting
```

## Files

| File | Description |
|------|-------------|
| `TwitchChatProvider.cs` | Main provider implementation |
| `TwitchChatConfiguration.cs` | Configuration model |
| `TwitchBadgeUrls.cs` | Badge URL helper |
| `TwitchChatStyles.css` | Default CSS styles |
| `HTML_FORMATTING.md` | HTML formatting documentation |
| `VISUAL_EXAMPLES.md` | Visual examples of formatted output |
| `IMPLEMENTATION_SUMMARY.md` | Implementation details |

## Examples

See [VISUAL_EXAMPLES.md](VISUAL_EXAMPLES.md) for visual examples of formatted messages.

## Dependencies

- TwitchLib.Client (3.4.0)
- TwitchLib.Communication (1.0.6)
- ChatProvider (project reference)

## Requirements

- .NET 10.0 or later
- Valid Twitch account for the bot
- OAuth token with appropriate scopes

## Troubleshooting

### Connection Issues
- Verify your OAuth token is valid
- Check that the channel name is correct (without #)
- Ensure the bot account has access to the channel

### Rate Limiting
- The provider enforces Twitch's rate limits automatically
- For verified bots, you can adjust the `MessagesAllowedInPeriod` setting

### Missing Emotes
- Ensure TwitchLib is properly receiving emote data
- Check that emote IDs are valid
- Verify CDN URLs are accessible

## Contributing

When contributing to this provider:
1. Maintain compatibility with the `IChatProvider` interface
2. Follow .NET coding conventions
3. Update documentation for new features
4. Test with actual Twitch chat connections

## License

See the main project license.

## Support

For issues specific to TwitchLib, see: https://github.com/TwitchLib/TwitchLib

## Version History

- v1.0.0 - Initial implementation
  - Basic chat connection
  - Message sending/receiving
  - HTML formatting with badges, emotes, and mentions
