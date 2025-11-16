using ChatProvider.Types;

namespace ChatProvider;

public class ChatHub
{
    private List<IChatProvider> _chatProviders = [];
    public async Task AddChatProvider(IChatProvider chatProvider)
    {
        _chatProviders.Add(chatProvider);
        chatProvider.OnChatRecieved = (msg) =>
        {
            if (OnMessageRecieved is not null)
            {
                OnMessageRecieved.Invoke(msg);
            }
        };
    }

    public Action<Message>? OnMessageRecieved { get; set; }
}
