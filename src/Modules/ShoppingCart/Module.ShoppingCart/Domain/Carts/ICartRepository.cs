using Contract.Abstractions;

namespace Module.ShoppingCart.Domain.Carts;

public interface ICartRepository : IRepository<Cart, Guid>
{
}
