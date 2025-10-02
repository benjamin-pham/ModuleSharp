using Contract.Infrastructure.Database;
using Contract.Utilities;
using Module.ShoppingCart.Domain.Carts;
using Module.ShoppingCart.Infrastructure.Database;

namespace Module.ShoppingCart.Infrastructure.Repositories;

[ServiceRegistration(ServiceLifetime.Scoped)]
public class CartRepository : BaseRepository<Cart, Guid, ShoppingCartDbContext>, ICartRepository
{
    public CartRepository(ShoppingCartDbContext dbContext) : base(dbContext)
    {
    }
}
