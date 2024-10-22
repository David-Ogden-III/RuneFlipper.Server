using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("buytypes")]
[Index("Name", Name = "buytypes_name_key", IsUnique = true)]
public partial class Buytype
{
    [Key]
    [Column("id")]
    [StringLength(3)]
    public string Id { get; set; } = null!;

    [Column("name")]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [InverseProperty("BuyType")]
    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
