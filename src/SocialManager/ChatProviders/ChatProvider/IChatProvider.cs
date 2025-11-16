using ChatProvider.Types;

namespace ChatProvider;

public interface IChatProvider
{
    string ChatPlatform { get; }

    Task Connect(CancellationToken ct);
    Task Disconnect(CancellationToken ct);

    Task SendMessageAsync(Message message, CancellationToken ct);

    Action<Message> OnChatRecieved { get; set; }
}
