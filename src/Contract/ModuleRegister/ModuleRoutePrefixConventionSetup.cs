using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Contract.ModuleRegister;

internal class ModuleRoutePrefixConventionSetup : IConfigureOptions<MvcOptions>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ModuleManager _moduleManager;
    public ModuleRoutePrefixConventionSetup(IServiceProvider serviceProvider, ModuleManager moduleManager)
    {
        _serviceProvider = serviceProvider;
        _moduleManager = moduleManager;
    }

    public void Configure(MvcOptions options)
    {
        options.Conventions.Insert(0, new ModuleRoutePrefixConvention(_serviceProvider, _moduleManager));
    }
}
