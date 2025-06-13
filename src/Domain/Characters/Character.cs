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
}
