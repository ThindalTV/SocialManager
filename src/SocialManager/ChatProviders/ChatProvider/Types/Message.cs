
namespace ChatProvider.Types;

public record Message
{
    public required ChatDirection Direction { get; init; }
    public required string ChatPlatform { get; init; }
    public required string Sender { get; init; }

    public TextMessage? TextMessage { get; init; }
    public EventMessage? EventMessage { get; init; }
    public RedeemMessage? RedeemMessage { get; init; }

    public DateTimeOffset Timestamp { get; init; }
}
