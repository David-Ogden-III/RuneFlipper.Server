using Models.DataTransferObjects;
using Models.Entities;

namespace Models.TradeFactory;
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

    public ItemResponse Item { get; set; } = new()
    {
        Id = trade.ItemId,
        InGameId = trade.Item.InGameId,
        Name = trade.Item.Name,
        Description = trade.Item.Description,
        MembersOnly = trade.Item.MembersOnly,
        TradeLimit = trade.Item.TradeLimit,
        ModeId = trade.Item.ModeId
    };
    public CharacterResponse Character { get; set; } = new()
    {
        Id = trade.CharacterId,
        Name = trade.Character.Name,
        Member = trade.Character.Member,
        UserId = trade.Character.UserId,
        CreatedAt = trade.Character.CreatedAt,
        ModeId = trade.Character.ModeId
    };
    public TransactionType BuyType { get; set; } = new(trade.BuyTypeId, trade.BuyType.Name);
    public TransactionType SellType { get; set; } = new(trade.SellTypeId, trade.SellType.Name);
}