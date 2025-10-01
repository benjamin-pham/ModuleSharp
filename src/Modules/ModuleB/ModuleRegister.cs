namespace ModuleB;

public class ModuleRegister : BaseModule
{
    public override string EndpointPrefix => "module-b";

    public override string[] ModuleSettingFiles => ["appsettings.ModuleB.json"];

    public override void ConfigureServices(WebApplicationBuilder builder)
    {

    }
}
