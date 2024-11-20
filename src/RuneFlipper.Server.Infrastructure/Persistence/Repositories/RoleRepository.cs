using Microsoft.AspNetCore.Identity;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class RoleRepository(RuneFlipperContext context) : GenericRepository<IdentityRole>(context)
    {
    }
}
