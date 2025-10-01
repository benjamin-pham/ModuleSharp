using Microsoft.AspNetCore.Builder;

namespace Contract;

public interface IModule
{
    string EndpointPrefix { get; }
    string[] ModuleSettingFiles { get; }
    void ConfigureServices(WebApplicationBuilder builder);
}
public abstract class BaseModule : IModule
{
    public abstract string EndpointPrefix { get; }
    public abstract string[] ModuleSettingFiles { get; }
    public abstract void ConfigureServices(WebApplicationBuilder builder);
}