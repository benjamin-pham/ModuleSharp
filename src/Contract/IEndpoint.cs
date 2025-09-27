using Microsoft.AspNetCore.Routing;

namespace Contract;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
