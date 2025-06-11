using Domain.Users;

namespace Domain.Permissions;
public sealed class Permission
{
    public static readonly Permission UsersAccesss = new(1, "users:access");

    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }
    public string Name { get; init; }

    public ICollection<User> Users { get; init; } = [];
}
