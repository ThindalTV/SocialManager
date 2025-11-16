namespace ChatProvider.Types.Events;

public record HostEvent
{
    public required string Hoster { get; init; }
    public int ViewerCount { get; init; }
}
