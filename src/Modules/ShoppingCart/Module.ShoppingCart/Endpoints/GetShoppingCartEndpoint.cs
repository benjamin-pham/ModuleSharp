using Contract.ModuleRegister;

namespace Module.ShoppingCart.Endpoints;

public class GetShoppingCartEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/carts", () => "cart data");
    }
}
