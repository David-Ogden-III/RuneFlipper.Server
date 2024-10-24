namespace Models.DataTransferObjects;

public record NewItem
{
    public required int InGameId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required bool MembersOnly { get; init; }
    public required int TradeLimit { get; init; }
    public required string ModeId { get; init; }
}
