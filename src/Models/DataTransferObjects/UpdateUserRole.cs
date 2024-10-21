namespace Models.DataTransferObjects;

public record UpdateUserRole(IEnumerable<string> RoleNames, string UserId);