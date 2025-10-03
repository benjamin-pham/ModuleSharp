using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contract.ModuleRegister;

internal class ModuleRoutePrefixConvention : IApplicationModelConvention
{
    private readonly Dictionary<Assembly, string> _endpointPrefixes;

    public ModuleRoutePrefixConvention(ModuleManager moduleManager)
    {
        _endpointPrefixes = moduleManager.AppModules
            .Distinct()
            .ToDictionary(
                t => t.Assembly,
                t =>
                {
                    return t.Instance.EndpointPrefix;
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