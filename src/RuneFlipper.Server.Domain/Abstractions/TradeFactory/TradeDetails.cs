using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Domain.Abstractions.TradeFactory;
public abstract class TradeDetails(Trade trade) : TradeCalculations
{
    public string Id { get; set; } = trade.Id;
    public long BuyPrice { get; set; } = trade.BuyPrice;
    public DateTime BuyDateTime { get; set; } = trade.BuyDateTime;
    public long SellPrice { get; set; } = trade.SellPrice;
    public DateTime SellDateTime { get; set; } = trade.SellDateTime;
    public int Quantity { get; set; } = trade.Quantity;
    public long BreakEvenPrice => CalculateGrossBreakEvenPrice(BuyPrice, trade.Item.InGameId);
    public long ProfitPerItem => CalculateNetProfitPerItem(SellPrice, trade.Item.InGameId, BuyPrice);
    public long TotalProfit => CalculateTotalNetProfit(SellPrice, trade.Item.InGameId, BuyPrice, Quantity);
    public long TaxPerItem => CalculateTaxPerItem(SellPrice, trade.Item.InGameId);
    public long TotalTax => CalculateTotalSalesTax(SellPrice, Quantity, trade.Item.InGameId);
    public bool IsComplete { get; set; } = trade.IsComplete;
    public abstract IItemResponse Item { get; set; }
    public abstract ICharacterResponse Character { get; set; }
    public abstract ITransactionType BuyType { get; set; }
    public abstract ITransactionType SellType { get; set; }
}