using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("items")]
public partial class Item
{
    [Key, Column("id"), StringLength(36)]
    public string Id { get; init; } = null!;


    [Column("ingameid")]
    public int InGameId { get; init; }


    [Column("name"), StringLength(255)]
    public string Name { get; init; } = null!;


    [Column("description")]
    public string Description { get; init; } = null!;


    [Column("member")]
    public bool MembersOnly { get; init; }


    [Column("tradelimit")]
    public int TradeLimit { get; init; }


    [Column("modeid")]
    [StringLength(8)]
    public string ModeId { get; init; } = null!;


    [ForeignKey("ModeId"), InverseProperty("Items")]
    public virtual Mode Mode { get; init; } = null!;


    [InverseProperty("Item")]
    public virtual ICollection<Trade> Trades { get; init; } = new List<Trade>();
}
