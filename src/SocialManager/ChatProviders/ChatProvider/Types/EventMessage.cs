using ChatProvider.Types.Events;

namespace ChatProvider.Types;

public record EventMessage
{
    public CheerEvent? CheerEvent { get; init; }
    public GiftedSubscriptionEvent? GiftedSubscriptionEvent { get; init; }
    public HostEvent? HostEvent { get; init; }
    public RaidEvent? RaidEvent { get; init; }
    public SubscribeEvent? SubscribeEvent { get; init; }
    public ResubscribeEvent? ResubscribeEvent { get; init; }
    public UserJoinedEvent? UserJoinedEvent { get; init; }
    public UserLeftEvent? UserLeftEvent { get; set; }
    public UserFollowedEvent? UserFollowedEvent { get; init; }
}
