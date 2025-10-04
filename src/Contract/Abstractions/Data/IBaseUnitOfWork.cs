namespace Contract.Abstractions.Data;

public interface IBaseUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
