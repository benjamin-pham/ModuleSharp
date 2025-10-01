
namespace WebHost;

public class ModuleRegister : BaseModule
{
    public override string EndpointPrefix => "web-host";

    public override string[] ModuleSettingFiles => ["appsettings.json"];

    public override void ConfigureServices(WebApplicationBuilder builder)
    {

    }
}
