using Contract.ModuleRegister;

namespace Module.Catalog.Endpoints;

public class GetProductsEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("/products", () => "123 456");
	}
}
