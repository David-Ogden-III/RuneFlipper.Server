using Domain.Todos;
using Domain.BuyTypes;
using Domain.Characters;
using Domain.Items;
using Domain.Modes;
using Domain.SellTypes;
using Domain.Trades;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<BuyType> BuyTypes { get; }
    DbSet<Character> Characters { get; }
    DbSet<Item> Items { get; }
    DbSet<Mode> Modes { get; }
    DbSet<SellType> SellTypes { get; }
    DbSet<Trade> Trades { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
