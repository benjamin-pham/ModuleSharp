using Contract.Infrastructure.Database;
using Contract.Utilities.Attributes;

using Module.Catalog.Domain.Products;
using Module.Catalog.Infrastructure.Database;

namespace Module.Catalog.Infrastructure.Repositories;

[ServiceRegister(ServiceLifetime.Scoped)]
public class ProductRepository : BaseRepository<Product, Guid, CatalogDbContext>, IProductRepository
{
    public ProductRepository(CatalogDbContext dbContext) : base(dbContext)
    {
    }
}
