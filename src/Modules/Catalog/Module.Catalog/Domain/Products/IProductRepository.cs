using Contract.Abstractions;

namespace Module.Catalog.Domain.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
}
