using Contract.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Module.Catalog.Infrastructure.Database;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options)
    : ApplicationDbContext<CatalogDbContext>(options)
{
}
