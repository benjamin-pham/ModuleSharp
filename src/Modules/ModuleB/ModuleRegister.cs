namespace ModuleB;

public class ModuleRegister : IModule
{
    public string EndpointPrefix => "module-b";

    public string[] ModuleSettingFiles => ["appsettings.ModuleB.json"];

    public void ConfigureServices(WebApplicationBuilder builder)
    {

    }
}
