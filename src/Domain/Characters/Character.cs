using Domain.Modes;
using Domain.Permissions;
using Domain.Trades;
using Domain.Users;
using SharedKernel;

namespace Domain.Characters;
public sealed class Character : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsMember { get; set; }
    public Guid UserId { get; set; }
    public int ModeId { get; set; }
    public DateTime CreatedAt { get; set; }

    public IReadOnlyCollection<Permission> Permissions => _permissions.ToList();
    private readonly List<Permission> _permissions = [];

    public User User { get; set; }
    public Mode Mode { get; set; }
    public ICollection<Trade> Trades { get; set; } = [];
}
