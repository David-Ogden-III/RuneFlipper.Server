using Microsoft.AspNetCore.Identity;
using Models;
using Models.DataTransferObjects;

namespace RuneFlipper.Server.Test;

public class ObjectMapperTest
{
    [Theory]
    [InlineData("")]
    [InlineData("Admin")]
    public void CreateRoleResponse_PropName_NameMatchesInputName(string propertyValue)
    {
        IdentityRole role = new()
        {
            Name = propertyValue,
        };

        RoleResponse roleResponse = ObjectMapper.CreateRoleResponse(role);

        Assert.Equal(role.Name, roleResponse.Name);
    }

    [Fact]
    public void CreateRoleResponse_NullName_NameIsEmptyString()
    {
        IdentityRole role = new();

        RoleResponse roleResponse = ObjectMapper.CreateRoleResponse(role);

        Assert.Empty(roleResponse.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ThisIsTheId")]
    public void CreateRoleResponse_PropId_IdMatchesInputId(string propertyValue)
    {
        IdentityRole role = new()
        {
            Id = propertyValue,
        };

        RoleResponse roleResponse = ObjectMapper.CreateRoleResponse(role);

        Assert.Equal(role.Id, roleResponse.Id);
    }

    [Fact]
    public void CreateRoleResponse_NullId_IdIsNotEmpty()
    {
        IdentityRole role = new();

        RoleResponse roleResponse = ObjectMapper.CreateRoleResponse(role);

        Assert.NotEmpty(roleResponse.Id);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void CreateRoleResponses_ArrayLength_NotEmpty(int length)
    {
        List<IdentityRole> roles = [];
        for (int i = 0; i < length; i++)
        {
            roles.Add(new IdentityRole());
        }

        var responses = ObjectMapper.CreateRoleResponses(roles);

        Assert.Equal(responses.Count(), length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void CreateRoleResponses_IndexToCompare_NameMatchesInput(int index)
    {
        IdentityRole[] roles = new IdentityRole[index + 1];
        for (int i = 0; i < index + 1; i++)
        {
            roles[i] = new IdentityRole()
            {
                Name = i.ToString()
            };
        }

        var responses = ObjectMapper.CreateRoleResponses(roles).ToArray();

        Assert.Equal(responses[index].Name, index.ToString());
    }
}
