namespace Contract.Abstractions;

public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);
    void Add(TEntity entity);
}
