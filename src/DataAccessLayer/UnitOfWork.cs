using Microsoft.AspNetCore.Identity;
using Models.Entities;

namespace DataAccessLayer;

public class UnitOfWork(RuneFlipperContext context) : IDisposable
{
    private readonly RuneFlipperContext _context = context;
    private GenericRepository<IdentityRole>? _roleRepository;
    private GenericRepository<Buytype>? _buyTypeRepository;
    private GenericRepository<Selltype>? _sellTypeRepository;
    private GenericRepository<Trade>? _tradeRepository;
    private GenericRepository<Item>? _itemRepository;
    private GenericRepository<Mode>? _modeRepository;
    private GenericRepository<Character>? _characterRepository;

    public GenericRepository<IdentityRole> RoleRepository
    {
        get
        {
            _roleRepository ??= new(_context);
            return _roleRepository;
        }
    }

    public GenericRepository<Buytype> BuytypeRepository
    {
        get
        {
            _buyTypeRepository ??= new(_context);
            return _buyTypeRepository;
        }
    }

    public GenericRepository<Selltype> SelltypeRepository
    {
        get
        {
            _sellTypeRepository ??= new(_context);
            return _sellTypeRepository;
        }
    }

    public GenericRepository<Trade> TradeRepository
    {
        get
        {
            _tradeRepository ??= new(_context);
            return _tradeRepository;
        }
    }

    public GenericRepository<Item> ItemRepository
    {
        get
        {
            _itemRepository ??= new(_context);
            return _itemRepository;
        }
    }

    public GenericRepository<Mode> ModeRepository
    {
        get
        {
            _modeRepository ??= new(_context);
            return _modeRepository;
        }
    }

    public GenericRepository<Character> CharacterRepository
    {
        get
        {
            _characterRepository ??= new(_context);
            return _characterRepository;
        }
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
