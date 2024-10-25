namespace Models.DataTransferObjects;

public record UpdateUserRole(ICollection<string> RoleNames, string UserId);