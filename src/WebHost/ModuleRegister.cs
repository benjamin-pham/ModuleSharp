
using Contract.ModuleRegister;

namespace WebHost;

public class ModuleRegister : IModule
{
    public string EndpointPrefix => "web-host";

    public string[] ModuleSettingFiles => ["appsettings.json"];

    public void ConfigureServices(WebApplicationBuilder builder)
    {

    }
}
