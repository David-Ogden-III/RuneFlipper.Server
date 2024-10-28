using Models.Entities;

namespace Models.TradeFactory;
public abstract class TradeSummary(Trade trade) : TradeCalculations
{
    public string Id { get; set; } = trade.Id;
    public string CharacterId { get; set; } = trade.CharacterId;
    public string ItemId { get; set; } = trade.ItemId;
    public int ItemInGameId { get ; set; } = trade.Item.InGameId;
    public string ItemName { get; set; } = trade.Item.Name;
    public long BuyPrice { get; set; } = trade.BuyPrice;
    public DateTime BuyDateTime { get; set; } = trade.BuyDateTime;
    public long SellPrice { get; set; } = trade.SellPrice;
    public DateTime SellDateTime { get; set;} = trade.SellDateTime;
    public int Quantity { get; set; } = trade.Quantity;
    public long TotalProfit => CalculateTotalNetProfit(SellPrice, ItemInGameId, BuyPrice, Quantity);
    public string BuyTypeShortName { get; set; } = trade.BuyType.Id;
    public string SellTypeShortName { get; set; } = trade.SellType.Id;
    public bool IsComplete { get; set; } = trade.IsComplete;
}