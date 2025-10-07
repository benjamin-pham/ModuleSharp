using Contract.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Module.ShoppingCart.Infrastructure.Database;

public class ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
    : ApplicationDbContext<ShoppingCartDbContext>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("shopping_cart");
    }
}
