using Contract.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contract;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWithAttributes(this IServiceCollection services, List<Type> moduleTypes)
    {
        var types = moduleTypes.Select(x => x.Assembly).SelectMany(x => x.GetTypes())
            .Where(t => t.IsClass
                     && !t.IsAbstract
                     && t.GetCustomAttribute<ServiceRegistrationAttribute>() != null);

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<ServiceRegistrationAttribute>();

            var allInterfaces = type.GetInterfaces();
            var serviceTypes = allInterfaces
                .Where(i => !allInterfaces.Any(other => other != i && other.GetInterfaces().Contains(i)))
                .ToArray();
            if (serviceTypes.Length == 0)
            {
                // Không có interface nào thì đăng ký luôn class chính
                serviceTypes = new[] { type };
            }

            foreach (var serviceType in serviceTypes)
            {
                services.Add(new ServiceDescriptor(serviceType, type, attr!.Lifetime));
            }
        }

        return services;
    }
}
