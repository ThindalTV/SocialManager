namespace ChatProvider.Types.Events;

public record UserFollowedEvent
{
    public required string UserName { get; init; }
}
