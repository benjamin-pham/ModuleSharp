namespace Contract.Abstractions;

public interface IBaseUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
