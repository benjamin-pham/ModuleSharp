using Contract.Infrastructure.Database;
using Contract.Utilities;
using Microsoft.EntityFrameworkCore;
using Module.Catalog.Domain.Products;
using Module.Catalog.Infrastructure.Database;

namespace Module.Catalog.Infrastructure.Repositories;

[ServiceRegistration(ServiceLifetime.Scoped)]
public class ProductRepository : BaseRepository<Product, Guid>, IProductRepository
{
    public ProductRepository(CatalogDbContext dbContext) : base(dbContext)
    {
    }
}
