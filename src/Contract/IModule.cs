using Microsoft.AspNetCore.Builder;

namespace Contract;

public interface IModule
{
    string EndpointPrefix { get; }
    string[] ModuleSettingFiles { get; }
    void ConfigureServices(WebApplicationBuilder builder);
}