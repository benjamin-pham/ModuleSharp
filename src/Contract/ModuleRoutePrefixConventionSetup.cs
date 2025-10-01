using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Contract;

internal class ModuleRoutePrefixConventionSetup : IConfigureOptions<MvcOptions>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<Type> _moduleTypes;
    public ModuleRoutePrefixConventionSetup(IServiceProvider serviceProvider, List<Type> moduleTypes)
    {
        _serviceProvider = serviceProvider;
        _moduleTypes = moduleTypes;
    }

    public void Configure(MvcOptions options)
    {
        options.Conventions.Insert(0, new ModuleRoutePrefixConvention(_serviceProvider, _moduleTypes));
    }
}
