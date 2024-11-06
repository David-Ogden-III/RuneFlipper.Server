using DataAccessLayer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

namespace RuneFlipper.Server.Test;

public class DataAccessLayerTest
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");

    [Fact]
    public void RuneFlipperContext_IsNull_ReturnsNotNull()
    {
        RuneFlipperContext context = new RuneFlipperContext();

        Assert.NotNull(context);
    }

    [Fact]
    public async Task GetAsync_IdentityRole_NotNull()
    {
        // Arrange
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<RuneFlipperContext>().UseSqlite(_connection).Options;

        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Roles.AddAsync(Role);
            await context.SaveChangesAsync();
        }

        // Act + Assert
        await using (RuneFlipperContext context = new(options))
        {
            UnitOfWork unitOfWork = new(context);

            Expression<Func<IdentityRole, bool>> filter = role => role.Id == Role.Id;
            var role = await unitOfWork.RoleRepository.GetAsync(filters: [filter]);

            Assert.NotNull(role);
        }

        // Cleanup
        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
        await _connection.CloseAsync();
    }

    [Fact]
    public async Task GetListAsync_SellType_NotNull()
    {
        // Arrange
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<RuneFlipperContext>().UseSqlite(_connection).Options;

        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Selltypes.AddRangeAsync(SellType1, SellType2);
            await context.SaveChangesAsync();
        }

        // Act + Assert
        await using (RuneFlipperContext context = new(options))
        {
            UnitOfWork unitOfWork = new(context);

            var sellType = await unitOfWork.SellTypeRepository.GetListAsync();

            Assert.Equal(2, sellType.Count);
        }

        // Cleanup
        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
        await _connection.CloseAsync();
    }

    [Fact]
    public async Task Insert_BuyType_EntityAdded()
    {
        // Arrange
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<RuneFlipperContext>().UseSqlite(_connection).Options;

        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureCreatedAsync();
            UnitOfWork unitOfWork = new(context);
            unitOfWork.BuyTypeRepository.Insert(BuyType);
            await unitOfWork.SaveAsync();
        }

        // Act + Assert
        await using (RuneFlipperContext context = new(options))
        {
            var buyType = await context.Buytypes.FindAsync(BuyType.Id);
            Assert.NotNull(buyType);
        }

        // Cleanup
        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
        await _connection.CloseAsync();
    }

    [Fact]
    public async Task Delete_Mode_ModeDeleted()
    {
        // Arrange
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<RuneFlipperContext>().UseSqlite(_connection).Options;

        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Modes.AddAsync(Mode);
            await context.SaveChangesAsync();
        }

        // Act + Assert
        await using (RuneFlipperContext context = new(options))
        {
            UnitOfWork unitOfWork = new(context);
            var mode = await context.Modes.FindAsync(Mode.Id);
            if (mode != null) unitOfWork.ModeRepository.Delete(mode);
            int numChanges = await unitOfWork.SaveAsync();

            Assert.Equal(1, numChanges);
        }

        // Cleanup
        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
        await _connection.CloseAsync();
    }

    [Fact]
    public async Task Update_Trade_BuyPriceUpdated()
    {
        // Arrange
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<RuneFlipperContext>().UseSqlite(_connection).Options;

        Trade trade = new()
        {
            Id = Guid.NewGuid().ToString(),
            BuyPrice = 100,
            SellPrice = 120,
            Quantity = 5,
            BuyDateTime = DateTime.Now,
            SellDateTime = DateTime.Now,
            IsComplete = true,
            CharacterId = Character.Id,
            ItemId = Item.Id,
            BuyTypeId = BuyType.Id,
            SellTypeId = SellType1.Id
        };

        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.AddAsync(SellType1);
            await context.AddAsync(BuyType);
            await context.AddAsync(Mode);
            await context.AddAsync(Item);
            await context.AddAsync(User);
            await context.AddAsync(Character);
            await context.AddAsync(trade);
            await context.SaveChangesAsync();
        }

        // Act
        await using (RuneFlipperContext context = new(options))
        {
            UnitOfWork unitOfWork = new(context);

            trade.BuyPrice = 120;

            unitOfWork.TradeRepository.Update(trade);
            await unitOfWork.SaveAsync();
        }

        // Assert
        await using (RuneFlipperContext context = new(options))
        {
            var updatedTrade = await context.Trades.FindAsync(trade.Id);
            Assert.NotNull(updatedTrade);
            Assert.Equal(120, trade.BuyPrice);
        }

        // Cleanup
        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
        await _connection.CloseAsync();
    }

    [Fact]
    public async Task Exists_CharacterExists_ReturnsTrue()
    {
        // Arrange
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<RuneFlipperContext>().UseSqlite(_connection).Options;

        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Characters.AddAsync(Character);
            await context.SaveChangesAsync();
        }

        // Act + Assert
        await using (RuneFlipperContext context = new(options))
        {
            UnitOfWork unitOfWork = new(context);

            Expression<Func<Character, bool>> filter = character => character.Id == Character.Id;
            bool exists = unitOfWork.CharacterRepository.Exists(filter);

            Assert.True(exists);
        }

        // Cleanup
        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
        await _connection.CloseAsync();
    }

    [Fact]
    public async Task Exists_ItemDoesNotExist_ReturnsFalse()
    {
        // Arrange
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<RuneFlipperContext>().UseSqlite(_connection).Options;

        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureCreatedAsync();
            await context.Items.AddAsync(Item);
            await context.SaveChangesAsync();
        }

        // Act + Assert
        await using (RuneFlipperContext context = new(options))
        {
            UnitOfWork unitOfWork = new(context);

            Expression<Func<Item, bool>> filter = item => item.Id == "Non-Existent Id";
            bool exists = unitOfWork.ItemRepository.Exists(filter);

            Assert.False(exists);
        }

        // Cleanup
        await using (RuneFlipperContext context = new(options))
        {
            await context.Database.EnsureDeletedAsync();
        }
        await _connection.CloseAsync();
    }

    // Predefined Entity Instances
    private static readonly SellType SellType1 = new()
    {
        Id = "NIS",
        Name = "Non-Instant Sell"
    };

    private static readonly SellType SellType2 = new()
    {
        Id = "INS",
        Name = "Instant Sell"
    };

    private static readonly BuyType BuyType = new()
    {
        Id = "NIB",
        Name = "Non-Instant Buy"
    };

    private static readonly Mode Mode = new()
    {
        Id = "RS",
        Name = "RuneScape"
    };

    private static readonly Item Item = new()
    {
        Id = Guid.NewGuid().ToString(),
        InGameId = 10,
        Name = "Item",
        Description = "Description",
        MembersOnly = true,
        TradeLimit = 10,
        ModeId = "RS"
    };

    private static readonly User User = new()
    {
        Id = "UserId"
    };

    private static readonly Character Character = new()
    {
        Id = "CharacterId",
        Name = "Character",
        ModeId = "RS",
        UserId = "UserId",
        CreatedAt = DateTime.Now,
        Member = true
    };

    private static readonly IdentityRole Role = new()
    {
        Id = "RoleId",
        Name = "Admin",
        NormalizedName = "ADMIN",
    };
}
