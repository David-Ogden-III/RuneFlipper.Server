using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RuneFlipper.Server.Models;

[Table("items")]
public partial class Item
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("ingameid")]
    public int InGameId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("member")]
    public bool Member { get; set; }

    [Column("tradelimit")]
    public int TradeLimit { get; set; }

    [Column("modeid")]
    [StringLength(8)]
    public string ModeId { get; set; } = null!;

    [ForeignKey("ModeId")]
    [InverseProperty("Items")]
    public virtual Mode Mode { get; set; } = null!;

    [InverseProperty("Item")]
    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
