using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class ItemRepository(RuneFlipperContext context) : GenericRepository<Item>(context)
    {
    }
}
