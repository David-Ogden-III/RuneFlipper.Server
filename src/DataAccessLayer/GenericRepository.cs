using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer;

public class GenericRepository<TEntity> where TEntity : class
{
    internal RuneFlipperContext _context;
    internal DbSet<TEntity> _dbSet;

    public GenericRepository(RuneFlipperContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<ICollection<TEntity>> Get(IEnumerable<Expression<Func<TEntity,
        bool>>> filters, IEnumerable<string> tablesToJoin,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (var filter in filters)
        {
            query = query.Where(filter);
        }

        foreach (var table in tablesToJoin)
        {
            query = query.Include(table);
        }

        List<TEntity> result;
        if (orderBy != null)
        {
            result = await orderBy(query).ToListAsync();
        }
        else
        {
            result = await query.ToListAsync();
        }

        return result;
    }

    public virtual void Insert(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Added;
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        bool entityExists = _dbSet.Any(predicate);
        return entityExists;
    }



}
