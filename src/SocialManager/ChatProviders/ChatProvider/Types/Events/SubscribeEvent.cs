namespace ChatProvider.Types.Events;

public record SubscribeEvent
{
    public required string Subscriber { get; init; }
    public int ResubCount { get; init; } = 0;
}
