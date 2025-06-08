using Application.Abstractions.Data;
using Domain.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider(IApplicationDbContext dbContext)
{
    public async Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        List<Permission> permissions = await dbContext.Users
            .Where(user => user.Id == userId)
            .SelectMany(user => user.Permissions)
            .ToListAsync();

        var permissionsSet = permissions.Select(p => p.Name).ToHashSet();

        return permissionsSet;
    }
}
