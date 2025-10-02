using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contract.ModuleRegister;

internal class ModuleRoutePrefixConvention : IApplicationModelConvention
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Assembly, string> _endpointPrefixes;

    public ModuleRoutePrefixConvention(IServiceProvider serviceProvider, IEnumerable<Type> moduleTypes)
    {
        _serviceProvider = serviceProvider;

        _endpointPrefixes = moduleTypes
            .Distinct()
            .ToDictionary(
                t => t.Assembly,
                t =>
                {
                    var instance = (IModule)ActivatorUtilities.CreateInstance(_serviceProvider, t);
                    return instance.EndpointPrefix;
                });
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            if (!_endpointPrefixes.TryGetValue(controller.ControllerType.Assembly, out var prefix))
                continue;

            foreach (var selector in controller.Selectors)
            {
                var routePrefix = new AttributeRouteModel(new RouteAttribute($"api/{prefix}"));

                selector.AttributeRouteModel = selector.AttributeRouteModel != null
                    ? AttributeRouteModel.CombineAttributeRouteModel(routePrefix, selector.AttributeRouteModel)
                    : AttributeRouteModel.CombineAttributeRouteModel(routePrefix, new AttributeRouteModel(new RouteAttribute("[controller]")));
            }
        }
    }
}