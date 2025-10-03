using Microsoft.Extensions.DependencyInjection;

namespace Contract.Utilities;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class InjectAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public InjectAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}