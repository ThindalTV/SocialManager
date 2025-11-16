namespace ChatProvider.Types.Events;

public record GiftedSubscriptionEvent
{
    public required string Gifter { get; init; }
    public required string Recipient { get; init; }
    public required int Tier { get; init; }
    public int TotalGiftCount { get; init; }
}
