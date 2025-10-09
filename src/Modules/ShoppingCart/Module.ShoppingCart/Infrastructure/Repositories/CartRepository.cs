using Contract.Infrastructure.Database;
using Contract.Utilities.Attributes;

using Module.ShoppingCart.Domain.Carts;
using Module.ShoppingCart.Infrastructure.Database;

namespace Module.ShoppingCart.Infrastructure.Repositories;

[ServiceRegister(ServiceLifetime.Scoped)]
public class CartRepository : BaseRepository<Cart, Guid, ShoppingCartDbContext>, ICartRepository
{
    public CartRepository(ShoppingCartDbContext dbContext) : base(dbContext)
    {
    }
}
