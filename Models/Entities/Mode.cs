using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("modes")]
[Index("Name", Name = "modes_name_key", IsUnique = true)]
public partial class Mode
{
    [Key]
    [Column("id")]
    [StringLength(8)]
    public string Id { get; set; } = null!;

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Mode")]
    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    [InverseProperty("Mode")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
