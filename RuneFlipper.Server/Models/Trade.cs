using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuneFlipper.Server.Models;

[Table("trades")]
public partial class Trade
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("buyprice")]
    public long BuyPrice { get; set; }

    [Column("sellprice")]
    public long SellPrice { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("buydatetime", TypeName = "timestamp without time zone")]
    public DateTime BuyDateTime { get; set; }

    [Column("selldatetime", TypeName = "timestamp without time zone")]
    public DateTime SellDateTime { get; set; }

    [Column("iscomplete")]
    public bool IsComplete { get; set; }

    [Column("characterid")]
    public string CharacterId { get; set; } = null!;

    [Column("itemid")]
    public string ItemId { get; set; } = null!;

    [Column("buytypeid")]
    [StringLength(3)]
    public string BuyTypeId { get; set; } = null!;

    [Column("selltypeid")]
    [StringLength(3)]
    public string SellTypeId { get; set; } = null!;

    [ForeignKey("BuyTypeId")]
    [InverseProperty("Trades")]
    public virtual Buytype BuyType { get; set; } = null!;

    [ForeignKey("CharacterId")]
    [InverseProperty("Trades")]
    public virtual Character Character { get; set; } = null!;

    [ForeignKey("ItemId")]
    [InverseProperty("Trades")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("SellTypeId")]
    [InverseProperty("Trades")]
    public virtual Selltype SellType { get; set; } = null!;
}
