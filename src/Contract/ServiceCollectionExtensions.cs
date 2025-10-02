using Contract.Abstractions;
using Contract.Infrastructure.Database;
using Contract.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                serviceTypes = [type];
            }

            foreach (var serviceType in serviceTypes)
            {
                services.Add(new ServiceDescriptor(serviceType, type, attr!.Lifetime));
            }
        }

        return services;
    }

    public static IServiceCollection AddApplicationDbContexts(
        this IServiceCollection services,
        IConfiguration configuration,
        List<Type> moduleTypes)
    {
        var dbContextTypes = moduleTypes.Select(x => x.Assembly).SelectMany(x => x.GetTypes())
            .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Where(t => t.BaseType is { IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(ApplicationDbContext<>))
            .ToList();

        foreach (var dbContextType in dbContextTypes)
        {
            var addDbContextMethod = typeof(EntityFrameworkServiceCollectionExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext)
                            && m.IsGenericMethodDefinition
                            && m.GetParameters().Any(p =>
                                p.ParameterType == typeof(Action<IServiceProvider, DbContextOptionsBuilder>)));

            var genericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);

            genericMethod.Invoke(
                null,
                new object?[]
                {
                    services,
                    Postgres.StandardOptions(configuration),
                    null,
                    null
                });

            var uowInterfaces = dbContextType.GetInterfaces()
                .Where(i => i != typeof(IBaseUnitOfWork) &&
                            typeof(IBaseUnitOfWork).IsAssignableFrom(i))
                .ToList();

            foreach (var uowInterface in uowInterfaces)
            {
                services.AddScoped(uowInterface, sp => sp.GetRequiredService(dbContextType));
            }
        }

        return services;
    }
}
