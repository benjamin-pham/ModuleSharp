using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Contract.ModuleRegister;

internal class ModuleRoutePrefixConvention : IApplicationModelConvention
{
    private readonly ModuleManager _moduleManager;

    public ModuleRoutePrefixConvention(ModuleManager moduleManager)
    {
        _moduleManager = moduleManager;
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var appModule = _moduleManager.AppModules
                .FirstOrDefault(m => m.Assembly == controller.ControllerType.Assembly);

            if (appModule == null)
                continue;

            foreach (var selector in controller.Selectors)
            {
                var routePrefix = new AttributeRouteModel(new RouteAttribute($"api/{appModule.Instance.EndpointPrefix}"));

                selector.AttributeRouteModel = selector.AttributeRouteModel != null
                    ? AttributeRouteModel.CombineAttributeRouteModel(routePrefix, selector.AttributeRouteModel)
                    : AttributeRouteModel.CombineAttributeRouteModel(routePrefix, new AttributeRouteModel(new RouteAttribute("[controller]")));
            }
        }
    }
}