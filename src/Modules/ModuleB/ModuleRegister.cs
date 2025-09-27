using System.Reflection;

namespace ModuleB;

public class ModuleRegister : BaseModule
{
    public override string EndpointPrefix => "module-b";

    public override string[] ModuleSettingFiles => ["appsettings.ModuleB.json"];

    protected override Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();

    public override void ConfigureServices(WebApplicationBuilder builder)
    {

    }
}
