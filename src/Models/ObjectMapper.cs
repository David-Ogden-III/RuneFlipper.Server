using Microsoft.AspNetCore.Identity;
using Models.DataTransferObjects;

namespace Models;

public static class ObjectMapper
{
    public static IEnumerable<RoleResponse> CreateFetchRoleResponses(IEnumerable<IdentityRole> roles)
    {
        List<RoleResponse> responseObjects = [];
        foreach (var role in roles)
        {
            RoleResponse newResponse = new()
            {
                Id = role.Id,
                Name = role.Name ?? String.Empty
            };
            responseObjects.Add(newResponse);
        }

        return responseObjects;
    }

    public static RoleResponse CreateFetchRoleResponse(IdentityRole role)
    {

        RoleResponse newResponse = new()
        {
            Id = role.Id,
            Name = role.Name ?? String.Empty
        };

        return newResponse;
    }
}
