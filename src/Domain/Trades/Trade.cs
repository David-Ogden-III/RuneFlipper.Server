using SharedKernel;

namespace Domain.Trades;
public sealed class Trade : Entity
{
    public Guid Id { get; set; }
    public long BuyPrice { get; set; }
    public long SellPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime BuyDateTime { get; set; }
    public DateTime SellDateTime { get; set; }
    public bool IsComplete { get; set; }
    public Guid CharacterId { get; set; }
    public int ItemId { get; set; }
    public int BuyTypeId { get; set; }
    public int SellTypeId { get; set; }
}
