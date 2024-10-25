using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("trades")]
public partial class Trade
{
    [Key]
    [Column("id")]
    [StringLength(36)]
    public string Id { get; init; } = null!;

    [Column("buyprice")]
    public long BuyPrice { get; init; }

    [Column("sellprice")]
    public long SellPrice { get; init; }

    [Column("quantity")]
    public int Quantity { get; init; }

    [Column("buydatetime", TypeName = "timestamp without time zone")]
    public DateTime BuyDateTime { get; init; }

    [Column("selldatetime", TypeName = "timestamp without time zone")]
    public DateTime SellDateTime { get; init; }

    [Column("iscomplete")]
    public bool IsComplete { get; init; }

    [Column("characterid")]
    [StringLength(36)]
    public string CharacterId { get; init; } = null!;

    [Column("itemid")]
    [StringLength(36)]
    public string ItemId { get; init; } = null!;

    [Column("buytypeid")]
    [StringLength(3)]
    public string BuyTypeId { get; init; } = null!;

    [Column("selltypeid")]
    [StringLength(3)]
    public string SellTypeId { get; init; } = null!;

    [ForeignKey("BuyTypeId")]
    [InverseProperty("Trades")]
    public virtual BuyType BuyType { get; init; } = null!;

    [ForeignKey("CharacterId")]
    [InverseProperty("Trades")]
    public virtual Character Character { get; init; } = null!;

    [ForeignKey("ItemId")]
    [InverseProperty("Trades")]
    public virtual Item Item { get; init; } = null!;

    [ForeignKey("SellTypeId")]
    [InverseProperty("Trades")]
    public virtual SellType SellType { get; init; } = null!;
}
