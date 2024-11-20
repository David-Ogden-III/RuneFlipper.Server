using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class CharacterRepository(RuneFlipperContext context) : GenericRepository<Character>(context)
    {
    }
}
