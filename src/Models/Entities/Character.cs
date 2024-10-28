using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("characters")]
public partial class Character
{
    [Key]
    [Column("id")]
    public string Id { get; init; } = null!;

    [Column("name")]
    [StringLength(12)]
    public string Name { get; init; } = null!;

    [Column("modeid")]
    [StringLength(8)]
    public string ModeId { get; init; } = null!;

    [Column("UserId")]
    public string UserId { get; init; } = null!;

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; init; }

    [Column("member")]
    public bool Member { get; init; }

    [ForeignKey("ModeId")]
    [InverseProperty("Characters")]
    public virtual Mode Mode { get; init; } = null!;

    [InverseProperty("Character")]
    public virtual ICollection<Trade> Trades { get; init; } = new List<Trade>();

    [ForeignKey("UserId")]
    [InverseProperty("Characters")]
    public virtual User User { get; init; } = null!;
}
