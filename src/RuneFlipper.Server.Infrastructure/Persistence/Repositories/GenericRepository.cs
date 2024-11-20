using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RuneFlipper.Server.Infrastructure.Persistence.Repositories;

public abstract class GenericRepository<TEntity>(RuneFlipperContext context) where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<ICollection<TEntity>> GetListAsync(ICollection<Expression<Func<TEntity, bool>>>? filters = null,
        ICollection<string>? tablesToJoin = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? limit = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }

        if (tablesToJoin != null)
        {
            foreach (var table in tablesToJoin)
            {
                query = query.Include(table);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (limit != null)
        {
            query = query.Take((int)limit);
        }

        List<TEntity> result = await query.ToListAsync();

        return result;
    }

    public async Task<TEntity?> GetAsync(ICollection<Expression<Func<TEntity, bool>>>? filters = null,
        ICollection<string>? tablesToJoin = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }

        if (tablesToJoin != null)
        {
            foreach (var table in tablesToJoin)
            {
                query = query.Include(table);
            }
        }

        var result = await query.FirstOrDefaultAsync();

        return result;
    }

    public void Insert(TEntity entity)
    {
        _dbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Added;
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        bool entityExists = _dbSet.Any(predicate);
        return entityExists;
    }
}
