using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class ModeRepository(RuneFlipperContext context) : GenericRepository<Mode>(context)
    {
    }
}
