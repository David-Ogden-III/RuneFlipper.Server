namespace Models.DataTransferObjects;

public class UpdateUserRole
{
    public IEnumerable<string> RoleNames { get; set; } = [];
    public string UserId { get; set; } = String.Empty;
}
