namespace ChatProvider.Types.Events;

public record UserLeftEvent
{
    public required string Username { get; init; }
}
