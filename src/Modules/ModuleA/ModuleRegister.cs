namespace ModuleA;

public class ModuleRegister : BaseModule
{
    public override string EndpointPrefix => "module-a";

    public override string[] ModuleSettingFiles => ["appsettings.ModuleA.json"];

    public override void ConfigureServices(WebApplicationBuilder builder)
    {
    }
}
