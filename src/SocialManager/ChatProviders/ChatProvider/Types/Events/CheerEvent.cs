namespace ChatProvider.Types.Events;

public record CheerEvent
{
    public required string Cheerer { get; init; }
    public required int Bits { get; init; }
    public string? Message { get; init; }
}
