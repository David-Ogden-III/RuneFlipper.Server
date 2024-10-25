using Microsoft.AspNetCore.Identity;
using Models.Entities;

namespace DataAccessLayer;

public class UnitOfWork(RuneFlipperContext context) : IDisposable
{
    private GenericRepository<IdentityRole>? _roleRepository;
    private GenericRepository<BuyType>? _buyTypeRepository;
    private GenericRepository<SellType>? _sellTypeRepository;
    private GenericRepository<Trade>? _tradeRepository;
    private GenericRepository<Item>? _itemRepository;
    private GenericRepository<Mode>? _modeRepository;
    private GenericRepository<Character>? _characterRepository;

    public GenericRepository<IdentityRole> RoleRepository
    {
        get
        {
            _roleRepository ??= new GenericRepository<IdentityRole>(context);
            return _roleRepository;
        }
    }

    public GenericRepository<BuyType> BuyTypeRepository
    {
        get
        {
            _buyTypeRepository ??= new GenericRepository<BuyType>(context);
            return _buyTypeRepository;
        }
    }

    public GenericRepository<SellType> SellTypeRepository
    {
        get
        {
            _sellTypeRepository ??= new GenericRepository<SellType>(context);
            return _sellTypeRepository;
        }
    }

    public GenericRepository<Trade> TradeRepository
    {
        get
        {
            _tradeRepository ??= new GenericRepository<Trade>(context);
            return _tradeRepository;
        }
    }

    public GenericRepository<Item> ItemRepository
    {
        get
        {
            _itemRepository ??= new GenericRepository<Item>(context);
            return _itemRepository;
        }
    }

    public GenericRepository<Mode> ModeRepository
    {
        get
        {
            _modeRepository ??= new GenericRepository<Mode>(context);
            return _modeRepository;
        }
    }

    public GenericRepository<Character> CharacterRepository
    {
        get
        {
            _characterRepository ??= new GenericRepository<Character>(context);
            return _characterRepository;
        }
    }

    public async Task<int> SaveAsync()
    {
        return await context.SaveChangesAsync();
    }

    private bool _disposed = false;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
