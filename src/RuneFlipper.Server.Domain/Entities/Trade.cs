using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuneFlipper.Server.Domain.Entities;

[Table("trades")]
public partial class Trade
{
    [Key, Column("id"), StringLength(36)]
    public string Id { get; set; } = null!;


    [Column("buyprice")]
    public long BuyPrice { get; set; }


    [Column("sellprice")]
    public long SellPrice { get; set; }


    [Column("quantity")]
    public int Quantity { get; set; }


    [Column("buydatetime", TypeName = "timestamp with time zone")]
    public DateTime BuyDateTime { get; set; }


    [Column("selldatetime", TypeName = "timestamp with time zone")]
    public DateTime SellDateTime { get; set; }


    [Column("iscomplete")]
    public bool IsComplete { get; set; }


    [Column("characterid"), StringLength(36)]
    public string CharacterId { get; set; } = null!;


    [Column("itemid"), StringLength(36)]
    public string ItemId { get; set; } = null!;


    [Column("buytypeid"), StringLength(3)]
    public string BuyTypeId { get; set; } = null!;


    [Column("selltypeid"), StringLength(3)]
    public string SellTypeId { get; set; } = null!;


    [ForeignKey("BuyTypeId"), InverseProperty("Trades")]
    public virtual BuyType BuyType { get; set; } = null!;


    [ForeignKey("CharacterId"), InverseProperty("Trades")]
    public virtual Character Character { get; set; } = null!;


    [ForeignKey("ItemId"), InverseProperty("Trades")]
    public virtual Item Item { get; set; } = null!;


    [ForeignKey("SellTypeId"), InverseProperty("Trades")]
    public virtual SellType SellType { get; set; } = null!;
}
