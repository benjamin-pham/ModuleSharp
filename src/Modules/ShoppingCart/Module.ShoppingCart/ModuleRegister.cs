using Contract.ModuleRegister;

namespace Module.ShoppingCart;

public class ModuleRegister : IModule
{
    public string EndpointPrefix => "shopping-cart";

    public string[] ModuleSettingFiles => ["appsettings.ShoppingCart.json"];

    public void ConfigureServices(WebApplicationBuilder builder)
    { }
}
