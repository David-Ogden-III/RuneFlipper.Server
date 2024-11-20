using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class SellTypeRepository(RuneFlipperContext context) : GenericRepository<SellType>(context)
    {
    }
}
