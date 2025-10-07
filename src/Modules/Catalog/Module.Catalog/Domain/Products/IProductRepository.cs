using Contract.Abstractions.Data;

namespace Module.Catalog.Domain.Products;

public interface IProductRepository : IRepository<Product, Guid>
{
}
