using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RuneFlipper.Server.Models;

[Table("selltypes")]
[Index("Name", Name = "selltypes_name_key", IsUnique = true)]
public partial class Selltype
{
    [Key]
    [Column("id")]
    [StringLength(3)]
    public string Id { get; set; } = null!;

    [Column("name")]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [InverseProperty("SellType")]
    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
