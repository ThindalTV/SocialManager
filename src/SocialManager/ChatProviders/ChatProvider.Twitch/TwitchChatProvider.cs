using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;
using TwitchLib.EventSub.Websockets;
using ChatProvider.Types.Events;
using TwitchLib.EventSub.Core.EventArgs.Channel;
using System.Text;

// Use aliases to avoid conflicts between TwitchLib and ChatProvider types
using TwitchChatMessage = TwitchLib.Client.Models.ChatMessage;
using ProviderChatMessage = ChatProvider.Types.Message;
using ProviderChatDirection = ChatProvider.Types.ChatDirection;
using ProviderTextMessage = ChatProvider.Types.TextMessage;
using ProviderEventMessage = ChatProvider.Types.EventMessage;


namespace ChatProvider.Twitch;

public class TwitchChatProvider : IChatProvider
{
    private TwitchClient? _client;
    private EventSubWebsocketClient? _eventSubClient;
    private TwitchChatConfiguration? _configuration;

    public string ChatPlatform => "Twitch";

    public Action<ProviderChatMessage> OnChatRecieved { get; set; } = null!;

    public TwitchChatProvider()
    {
    }

    public void Configure(TwitchChatConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task Connect(CancellationToken ct)
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("TwitchChatProvider must be configured before connecting. Call Configure() first.");
        }

        var credentials = new ConnectionCredentials(_configuration.BotUsername, _configuration.AccessToken);
        var clientOptions = new ClientOptions();
        var customClient = new WebSocketClient(clientOptions);
        _client = new TwitchClient(customClient);

        _client.Initialize(credentials, _configuration.Channel);

        // Subscribe to message events
        _client.OnMessageReceived += OnMessageReceived;
        _client.OnConnected += OnConnected;
        _client.OnDisconnected += OnDisconnected;
        _client.OnError += OnError;

        // Subscribe to subscription events
        _client.OnNewSubscriber += OnNewSubscriber;
        _client.OnReSubscriber += OnReSubscriber;
        _client.OnGiftedSubscription += OnGiftedSubscription;

        // Subscribe to community events
        _client.OnRaidNotification += OnRaidNotification;
        // Subscribe to user events
        _client.OnUserJoined += OnUserJoined;
        _client.OnUserLeft += OnUserLeft;

        // Subscribe to community subscription events
        _client.OnCommunitySubscription += OnCommunitySubscription;

        _client.Connect();

        // Initialize EventSub WebSocket for channel point redemptions and other events if ChannelId is provided
        if (!string.IsNullOrEmpty(_configuration.ChannelId))
        {
            _eventSubClient = new EventSubWebsocketClient();

            // Subscribe to EventSub events
            _eventSubClient.ChannelPointsCustomRewardRedemptionAdd += OnChannelPointsRewardRedeemed;
            _eventSubClient.ChannelFollow += OnChannelFollow;

            // Connect to EventSub WebSocket
            await _eventSubClient.ConnectAsync();
        }
    }

    public async Task Disconnect(CancellationToken ct)
    {
        if (_client != null && _client.IsConnected)
        {
            _client.Disconnect();
        }

        if (_eventSubClient != null)
        {
            await _eventSubClient.DisconnectAsync();
        }
    }

    public Task SendMessageAsync(ProviderChatMessage message, CancellationToken ct)
    {
        if (_client == null || !_client.IsConnected)
        {
            throw new InvalidOperationException("Cannot send message: Client is not connected.");
        }

        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration is not set.");
        }

        if (message.TextMessage == null)
        {
            throw new InvalidOperationException("Cannot send message: TextMessage is null.");
        }

        _client.SendMessage(_configuration.Channel, message.TextMessage.Content);

        return Task.CompletedTask;
    }

    private void OnMessageReceived(object? sender, OnMessageReceivedArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = e.ChatMessage.DisplayName,
            TextMessage = new ProviderTextMessage
            {
                Content = e.ChatMessage.Message,
                ContentHtml = FormatMessageAsHtml(e.ChatMessage)
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private void OnConnected(object? sender, OnConnectedArgs e)
    {
        // Optional: Log connection established
    }

    private void OnDisconnected(object? sender, OnDisconnectedEventArgs e)
    {
        // Optional: Handle disconnection
    }

    private void OnError(object? sender, OnErrorEventArgs e)
    {
        // Optional: Handle errors
    }

    private void OnNewSubscriber(object? sender, OnNewSubscriberArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                SubscribeEvent = new SubscribeEvent
                {
                    Subscriber = e.Subscriber.DisplayName
                }
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private void OnReSubscriber(object? sender, OnReSubscriberArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                ResubscribeEvent = new ResubscribeEvent
                {
                    Subscriber = e.ReSubscriber.DisplayName,
                    Months = e.ReSubscriber.Months,
                    StreakMonths = 0, // TwitchLib doesn't provide StreakMonths in this version
                    Message = e.ReSubscriber.ResubMessage
                }
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private void OnGiftedSubscription(object? sender, OnGiftedSubscriptionArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                GiftedSubscriptionEvent = new GiftedSubscriptionEvent
                {
                    Gifter = e.GiftedSubscription.DisplayName,
                    Recipient = e.GiftedSubscription.MsgParamRecipientDisplayName,
                    Tier = ConvertSubscriptionPlanToTier(e.GiftedSubscription.MsgParamSubPlan),
                    TotalGiftCount = 1 // Single gift
                }
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private void OnRaidNotification(object? sender, OnRaidNotificationArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                RaidEvent = new RaidEvent
                {
                    Raider = e.RaidNotification.DisplayName,
                    ViewerCount = int.Parse(e.RaidNotification.MsgParamViewerCount)
                }
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private void OnUserJoined(object? sender, OnUserJoinedArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                UserJoinedEvent = new UserJoinedEvent
                {
                    Username = e.Username
                }
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private void OnUserLeft(object? sender, OnUserLeftArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                UserLeftEvent = new UserLeftEvent() { Username = e.Username }
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private void OnCommunitySubscription(object? sender, OnCommunitySubscriptionArgs e)
    {
        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                GiftedSubscriptionEvent = new GiftedSubscriptionEvent
                {
                    Gifter = e.GiftedSubscription.DisplayName,
                    Recipient = "Community",
                    Tier = ConvertSubscriptionPlanToTier(e.GiftedSubscription.MsgParamSubPlan),
                    TotalGiftCount = e.GiftedSubscription.MsgParamMassGiftCount
                }
            },
            Timestamp = DateTime.UtcNow
        };

        OnChatRecieved?.Invoke(chatMessage);
    }

    private Task OnChannelPointsRewardRedeemed(object? sender, ChannelPointsCustomRewardRedemptionArgs e)
    {
        var reward = e.Payload.Event;

        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            RedeemMessage = new Types.RedeemMessage
            {
                Redeemer = reward.UserName,
                RedeemTitle = reward.Reward.Title,
                RedeemDetails = reward.UserInput ?? string.Empty
            },
            Timestamp = reward.RedeemedAt
        };

        OnChatRecieved?.Invoke(chatMessage);
        return Task.CompletedTask;
    }

    private Task OnChannelFollow(object? sender, ChannelFollowArgs e)
    {
        var follow = e.Payload.Event;

        var chatMessage = new ProviderChatMessage
        {
            Direction = ProviderChatDirection.Recieved,
            ChatPlatform = ChatPlatform,
            Sender = "Twitch",
            EventMessage = new ProviderEventMessage()
            {
                UserFollowedEvent = new UserFollowedEvent
                {
                    UserName = follow.UserName
                }
            },
            Timestamp = follow.FollowedAt
        };

        OnChatRecieved?.Invoke(chatMessage);
        return Task.CompletedTask;
    }

    private int ConvertSubscriptionPlanToTier(TwitchLib.Client.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            TwitchLib.Client.Enums.SubscriptionPlan.Prime => 1,
            TwitchLib.Client.Enums.SubscriptionPlan.Tier1 => 1,
            TwitchLib.Client.Enums.SubscriptionPlan.Tier2 => 2,
            TwitchLib.Client.Enums.SubscriptionPlan.Tier3 => 3,
            _ => 1
        };
    }

    private string FormatMessageAsHtml(TwitchChatMessage chatMessage)
    {
        var html = new StringBuilder();

        // Add badges if present
        if (chatMessage.Badges.Count > 0)
        {
            html.Append("<span class=\"twitch-badges\">");
            foreach (var badge in chatMessage.Badges)
            {
                // Badge format: name/version (e.g., "moderator/1", "subscriber/12")
                var badgeName = badge.Key;
                var badgeVersion = badge.Value;
                html.Append($"<span class=\"twitch-badge twitch-badge-{System.Net.WebUtility.HtmlEncode(badgeName)}\" data-version=\"{System.Net.WebUtility.HtmlEncode(badgeVersion)}\" title=\"{System.Net.WebUtility.HtmlEncode(badgeName)}\"></span>");
            }
            html.Append("</span> ");
        }

        // Process the message text with emotes and mentions
        var messageText = chatMessage.Message;
        var processedMessage = ProcessMessageWithEmotesAndMentions(messageText, chatMessage);

        html.Append(processedMessage);

        return html.ToString();
    }

    private string ProcessMessageWithEmotesAndMentions(string message, TwitchChatMessage chatMessage)
    {
        // Create a list of replacements to make (position-based)
        var replacements = new List<(int Start, int End, string Replacement)>();

        // Process emotes (TwitchLib provides emote positions)
        if (chatMessage.EmoteSet?.Emotes != null)
        {
            foreach (var emote in chatMessage.EmoteSet.Emotes)
            {
                // Emote URL format: https://static-cdn.jtvnw.net/emoticons/v2/{emote.Id}/default/dark/1.0
                var emoteUrl = $"https://static-cdn.jtvnw.net/emoticons/v2/{emote.Id}/default/dark/1.0";
                var emoteHtml = $"<img src=\"{System.Net.WebUtility.HtmlEncode(emoteUrl)}\" alt=\"{System.Net.WebUtility.HtmlEncode(emote.Name)}\" title=\"{System.Net.WebUtility.HtmlEncode(emote.Name)}\" class=\"twitch-emote\" />";

                replacements.Add((emote.StartIndex, emote.EndIndex, emoteHtml));
            }
        }

        // Sort replacements by start position in descending order to replace from end to start
        replacements = replacements.OrderByDescending(r => r.Start).ToList();

        // Build the final HTML by replacing from the end to avoid index shifting
        var result = new StringBuilder(message);
        foreach (var (start, end, replacement) in replacements)
        {
            // Remove the original text and insert the replacement
            var length = end - start + 1;
            result.Remove(start, length);
            result.Insert(start, replacement);
        }

        // Process mentions (words starting with @)
        var finalText = result.ToString();
        var words = finalText.Split(' ');
        var processedWords = new List<string>();

        foreach (var word in words)
        {
            // Check if word starts with @ and is not already HTML
            if (word.StartsWith('@') && !word.StartsWith("<"))
            {
                var username = word.TrimStart('@');
                // Remove trailing punctuation for the mention highlight
                var punctuation = "";
                while (username.Length > 0 && char.IsPunctuation(username[^1]))
                {
                    punctuation = username[^1] + punctuation;
                    username = username[..^1];
                }

                if (!string.IsNullOrWhiteSpace(username))
                {
                    var mentionHtml = $"<span class=\"twitch-mention\">@{System.Net.WebUtility.HtmlEncode(username)}</span>{System.Net.WebUtility.HtmlEncode(punctuation)}";
                    processedWords.Add(mentionHtml);
                }
                else
                {
                    processedWords.Add(System.Net.WebUtility.HtmlEncode(word));
                }
            }
            else
            {
                // Only HTML encode if it's not already HTML markup
                if (!word.StartsWith("<") && !word.EndsWith(">"))
                {
                    processedWords.Add(System.Net.WebUtility.HtmlEncode(word));
                }
                else
                {
                    processedWords.Add(word);
                }
            }
        }

        return string.Join(' ', processedWords);
    }
}
