using Microsoft.Extensions.DependencyInjection;

namespace Contract.Utilities;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceRegistrationAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public ServiceRegistrationAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}