using Domain.BuyTypes;
using Domain.Characters;
using Domain.Items;
using Domain.SellTypes;
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
    public string CharacterId { get; set; }
    public string ItemId { get; set; }
    public string BuyTypeId { get; set; }
    public string SellTypeId { get; set; }

    public BuyType BuyType { get; set; }
    public Character Character { get; set; }
    public Item Item { get; set; }
    public SellType SellType { get; set; }
}
