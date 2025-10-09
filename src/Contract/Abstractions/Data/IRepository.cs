namespace Contract.Abstractions.Data;

public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : notnull
{
    Task<TEntity?> GetByIdAsync(TKey id);
    void Add(TEntity entity);
}
