using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class TradeRepository(RuneFlipperContext context) : GenericRepository<Trade>(context)
    {
    }
}
