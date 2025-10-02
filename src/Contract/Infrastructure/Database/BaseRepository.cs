using Contract.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Contract.Infrastructure.Database;

public class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    private readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }

    public Task<TEntity?> GetByIdAsync(TKey id)
    {
        return _dbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.Id!.Equals(id));
    }
}
