using Domain.Permissions;
using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }

    public IReadOnlyCollection<Permission> Permissions => _permissions.ToList();

    private readonly List<Permission> _permissions = [];
}
