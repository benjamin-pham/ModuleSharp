using Contract.Infrastructure.Database;
using Contract.ModuleRegister;
using Module.ShoppingCart.Infrastructure.Database;

namespace Module.ShoppingCart;

public class ModuleRegister : IModule
{
    public string EndpointPrefix => "shopping-cart";

    public string[] ModuleSettingFiles => ["appsettings.ShoppingCart.json"];

    public void ConfigureServices(WebApplicationBuilder builder)
    {

    }
}
