namespace ChatProvider.Types.Events;

public record UserJoinedEvent
{
    public required string Username { get; init; }
}
