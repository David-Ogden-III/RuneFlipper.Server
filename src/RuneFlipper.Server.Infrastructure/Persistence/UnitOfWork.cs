using RuneFlipper.Server.Infrastructure.Persistence.Repositories;

namespace RuneFlipper.Server.Infrastructure.Persistence;

public class UnitOfWork(RuneFlipperContext context) : IDisposable
{
    private RoleRepository? _roleRepository;
    private BuyTypeRepository? _buyTypeRepository;
    private SellTypeRepository? _sellTypeRepository;
    private TradeRepository? _tradeRepository;
    private ItemRepository? _itemRepository;
    private ModeRepository? _modeRepository;
    private CharacterRepository? _characterRepository;

    public RoleRepository RoleRepository
    {
        get
        {
            _roleRepository ??= new RoleRepository(context);
            return _roleRepository;
        }
    }

    public BuyTypeRepository BuyTypeRepository
    {
        get
        {
            _buyTypeRepository ??= new BuyTypeRepository(context);
            return _buyTypeRepository;
        }
    }

    public SellTypeRepository SellTypeRepository
    {
        get
        {
            _sellTypeRepository ??= new SellTypeRepository(context);
            return _sellTypeRepository;
        }
    }

    public TradeRepository TradeRepository
    {
        get
        {
            _tradeRepository ??= new TradeRepository(context);
            return _tradeRepository;
        }
    }

    public ItemRepository ItemRepository
    {
        get
        {
            _itemRepository ??= new ItemRepository(context);
            return _itemRepository;
        }
    }

    public ModeRepository ModeRepository
    {
        get
        {
            _modeRepository ??= new ModeRepository(context);
            return _modeRepository;
        }
    }

    public CharacterRepository CharacterRepository
    {
        get
        {
            _characterRepository ??= new CharacterRepository(context);
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
