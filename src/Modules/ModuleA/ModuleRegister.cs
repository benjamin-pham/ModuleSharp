namespace ModuleA;

public class ModuleRegister : IModule
{
    public string EndpointPrefix => "module-a";

    public string[] ModuleSettingFiles => ["appsettings.ModuleA.json"];

    public void ConfigureServices(WebApplicationBuilder builder)
    {
    }
}
