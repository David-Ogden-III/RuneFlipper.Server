using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("selltypes")]
[Index("Name", Name = "selltypes_name_key", IsUnique = true)]
public partial class SellType
{
    [Key, Column("id"), StringLength(3)]
    public string Id { get; init; } = null!;


    [Column("name"), StringLength(20)]
    public string Name { get; init; } = null!;


    [InverseProperty("SellType")]
    public virtual ICollection<Trade> Trades { get; init; } = new List<Trade>();
}
