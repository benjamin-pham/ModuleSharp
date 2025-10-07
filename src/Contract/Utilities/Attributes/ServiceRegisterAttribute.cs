using Microsoft.Extensions.DependencyInjection;

namespace Contract.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceRegisterAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public ServiceRegisterAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}