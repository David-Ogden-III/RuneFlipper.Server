using Microsoft.AspNetCore.Identity;
using Models.DataTransferObjects;

namespace Models;

public static class ObjectMapper
{
    public static IEnumerable<FetchRoleResponse> CreateFetchRoleResponses(IEnumerable<IdentityRole> roles)
    {
        List<FetchRoleResponse> responseObjects = [];
        foreach (var role in roles)
        {
            FetchRoleResponse newResponse = new()
            {
                Id = role.Id,
                Name = role.Name ?? String.Empty
            };
            responseObjects.Add(newResponse);
        }

        return responseObjects;
    }

    public static FetchRoleResponse CreateFetchRoleResponse(IdentityRole role)
    {

        FetchRoleResponse newResponse = new()
        {
            Id = role.Id,
            Name = role.Name ?? String.Empty
        };

        return newResponse;
    }
}
