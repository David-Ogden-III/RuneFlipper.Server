using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("characters")]
public partial class Character
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("name")]
    [StringLength(12)]
    public string Name { get; set; } = null!;

    [Column("modeid")]
    [StringLength(8)]
    public string ModeId { get; set; } = null!;

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("member")]
    public bool Member { get; set; }

    [ForeignKey("ModeId")]
    [InverseProperty("Characters")]
    public virtual Mode Mode { get; set; } = null!;

    [InverseProperty("Character")]
    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();

    [ForeignKey("UserId")]
    [InverseProperty("Characters")]
    public virtual User User { get; set; } = null!;
}
