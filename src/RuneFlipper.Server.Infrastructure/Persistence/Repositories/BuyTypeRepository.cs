using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class BuyTypeRepository(RuneFlipperContext context) : GenericRepository<BuyType>(context)
    {
    }
}
