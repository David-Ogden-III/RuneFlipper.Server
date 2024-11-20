namespace RuneFlipper.Server.Application.Roles.TransferObjects;

public record UpdateUserRole(ICollection<string> RoleNames, string UserId);