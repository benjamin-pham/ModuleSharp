using Contract.ModuleRegister;
namespace Module.Catalog;

public class ModuleRegister : IModule
{
	public string EndpointPrefix => "catalog";

	public string[] ModuleSettingFiles => ["appsettings.Catalog.json"];

	public void ConfigureServices(WebApplicationBuilder builder)
	{

	}
}
