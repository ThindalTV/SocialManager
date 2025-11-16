namespace ChatProvider.Types.Events;

public record RaidEvent
{
    public required string Raider { get; init; }
    public required int ViewerCount { get; init; }
}
