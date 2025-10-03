using System.Reflection;

namespace Contract.ModuleRegister;

public class ModuleManager(List<ModuleManager.AppModule> appModules)
{
    public List<AppModule> AppModules { get; } = appModules;

    public ModuleManager Add(AppModule module)
    {
        AppModules.Add(module);
        return this;
    }

    public sealed class AppModule(Assembly assembly, IModule instance)
    {
        public Assembly Assembly { get; } = assembly;
        public IModule Instance { get; } = instance;
        public Type[] AssemblyTypes { get; } = assembly.GetTypes();
    }
}
