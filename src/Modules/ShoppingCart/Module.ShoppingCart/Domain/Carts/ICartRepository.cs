using Contract.Abstractions.Data;

namespace Module.ShoppingCart.Domain.Carts;

public interface ICartRepository : IRepository<Cart, Guid>
{
}
