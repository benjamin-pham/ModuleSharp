
using System.Reflection;

namespace ModuleA;

public class ModuleRegister : BaseModule
{
    public override string EndpointPrefix => "module-a";

    public override string[] ModuleSettingFiles => ["appsettings.ModuleA.json"];

    protected override Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();

    public override void ConfigureServices(WebApplicationBuilder builder)
    {
    }
}
