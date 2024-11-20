namespace RuneFlipper.Server.Application.Trades.TransferObjects;
public record UpdateTradeRequest
{
    public required string Id { get; init; }
    public required int Quantity { get; init; }
    public required long BuyPrice { get; init; }
    public required long SellPrice { get; init; }
    public required string BuyTypeId { get; init; }
    public required string SellTypeId { get; init; }
    public required string CharacterId { get; init; }
    public required string ItemId { get; init; }
    public required bool IsComplete { get; init; }
    public required DateTime BuyDateTime { get; init; }
    public required DateTime SellDateTime { get; init; }
}