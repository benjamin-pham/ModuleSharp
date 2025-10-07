using Contract.Abstractions;
using Contract.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Contract.Infrastructure.Database;

public class BaseRepository<TEntity, TKey, TDbContext> : IRepository<TEntity, TKey> where TEntity : Entity<TKey> where TDbContext : DbContext where TKey : notnull
{
    private readonly TDbContext _dbContext;

    public BaseRepository(TDbContext dbContext)
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
