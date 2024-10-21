using Microsoft.AspNetCore.Identity;
using Models.DataTransferObjects;

namespace Models;

public static class ObjectMapper
{
    public static IEnumerable<RoleResponse> CreateRoleResponses(IEnumerable<IdentityRole> roles)
    {
        List<RoleResponse> responseObjects = [];
        foreach (var role in roles)
        {
            RoleResponse newResponse = new(role.Id, role.Name ?? String.Empty);
            responseObjects.Add(newResponse);
        }

        return responseObjects;
    }
}
