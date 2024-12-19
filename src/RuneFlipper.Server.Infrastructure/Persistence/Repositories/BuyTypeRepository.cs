using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RuneFlipper.Server.Domain.Entities;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories
{
    public class BuyTypeRepository(RuneFlipperContext context) : GenericRepository<BuyType>(context)
    {
        private readonly DbSet<BuyType> _dbSet = context.Set<BuyType>();

        public async Task<BuyType?> GetByIdAsync(string buyTypeId)
        {
            var result = await context.Buytypes.Where(buyType => buyType.Id == buyTypeId).FirstOrDefaultAsync();

            return result;
        }
    }
}
