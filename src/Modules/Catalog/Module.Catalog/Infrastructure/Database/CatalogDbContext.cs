using Contract.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

using Module.Catalog.Abstractions;

namespace Module.Catalog.Infrastructure.Database;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options)
    : ApplicationDbContext<CatalogDbContext>(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");
    }
}
