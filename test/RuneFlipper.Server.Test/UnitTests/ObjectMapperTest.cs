using Microsoft.AspNetCore.Identity;
using Models;

namespace RuneFlipper.Server.Test.UnitTests;

public class ObjectMapperTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void CreateRoleResponses_ArrayLength_NotEmpty(int length)
    {
        IdentityRole[] roles = new IdentityRole[length];
        for (int i = 0; i < length; i++)
        {
            roles[i] = new IdentityRole();
        }

        var responses = ObjectMapper.CreateRoleResponses(roles);

        Assert.Equal(responses.Count, length);
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
            roles[i] = new IdentityRole
            {
                Name = i.ToString()
            };
        }

        var responses = ObjectMapper.CreateRoleResponses(roles).ToArray();

        Assert.Equal(responses[index].Name, index.ToString());
    }

    [Fact]
    public void GetInstance_ObjectMapperNotInstantiated_ReturnsInstance()
    {
        ObjectMapper objectMapper = ObjectMapper.GetInstance();

        Assert.NotNull(objectMapper);
    }
}
