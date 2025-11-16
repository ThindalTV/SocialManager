namespace ChatProvider.Types;

public record RedeemMessage
{
    public required string Redeemer { get; init; }

    public required string RedeemTitle { get; init; }
    public required string RedeemDetails { get; init; }
}
