using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Contract;

public interface IModule
{
    string EndpointPrefix { get; }
    string[] ModuleSettingFiles { get; }
    void ConfigureServices(WebApplicationBuilder builder);
    void ConfigureEndpoints(IEndpointRouteBuilder endpoint);
}
public abstract class BaseModule : IModule
{
    protected abstract Assembly ExecutingAssembly { get; }
    public abstract string EndpointPrefix { get; }
    public abstract string[] ModuleSettingFiles { get; }
    public abstract void ConfigureServices(WebApplicationBuilder builder);
    public virtual void ConfigureEndpoints(IEndpointRouteBuilder endpoint)
    {
        var endpointTypes = ExecutingAssembly.GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).ToList();

        foreach (var type in endpointTypes)
        {
            var endpointInstance = (IEndpoint)Activator.CreateInstance(type)!;
            endpointInstance.MapEndpoint(endpoint);
        }
    }
}