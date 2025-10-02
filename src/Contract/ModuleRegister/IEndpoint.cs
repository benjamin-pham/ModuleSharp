using Microsoft.AspNetCore.Routing;

namespace Contract.ModuleRegister;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
