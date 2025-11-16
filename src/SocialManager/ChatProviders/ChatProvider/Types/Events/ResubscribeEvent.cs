namespace ChatProvider.Types.Events;

public record ResubscribeEvent
{
    public required string Subscriber { get; init; }
    public required int Months { get; init; }
    public int StreakMonths { get; init; }
    public string? Message { get; init; }
}
