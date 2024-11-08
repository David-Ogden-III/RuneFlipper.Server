using Microsoft.AspNetCore.Identity;
using Models;
using Models.DataTransferObjects;
using Models.Entities;

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

    [Fact]
    public void CreateNewTrade_InputNewTrade_TradeNotNull()
    {
        NewTrade newTrade = new()
        {
            CharacterId = "CharacterId",
            ItemId = "ItemId",
            BuyTypeId = "BuyTypeId",
            SellTypeId = "SellTypeId",
            BuyPrice = 100,
            SellPrice = 120,
            Quantity = 10,
            IsComplete = true,
            BuyDateTime = DateTime.Now,
            SellDateTime = DateTime.Now + TimeSpan.FromHours(1)
        };

        Trade trade = ObjectMapper.CreateNewTrade(newTrade);

        Assert.NotNull(trade);
    }
}
