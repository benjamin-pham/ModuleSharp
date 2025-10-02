using Contract.ModuleRegister;

namespace ModuleA;

public class GetProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", () => "get product");
    }
}
