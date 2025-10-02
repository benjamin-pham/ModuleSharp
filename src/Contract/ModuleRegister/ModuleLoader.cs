using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.Loader;

namespace Contract.ModuleRegister;

public static class ModuleLoader
{
    public static void AddHostConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        var moduleTypes = builder.AddModuleServices();

        builder.Services.AddTransient<IConfigureOptions<MvcOptions>>((serviceProvider) =>
        {
            return new ModuleRoutePrefixConventionSetup(serviceProvider, moduleTypes);
        });
    }

    public static void UseHostConfigure(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapModuleEndpoints();

        app.MapFallback(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Resource not found");
        });
    }

    private static List<Type> AddModuleServices(this WebApplicationBuilder builder)
    {
        var rootPath = AppContext.BaseDirectory;

        var assemblies = LoadAssemblies(rootPath);

        var moduleTypes = FindModuleTypes(assemblies);

        ConfigureModuleServices(builder, moduleTypes);

        builder.Services.AddSingleton(new ModuleType(moduleTypes));

        return moduleTypes;
    }

    private static void MapModuleEndpoints(this WebApplication app)
    {
        var moduleType = app.Services.GetRequiredService<ModuleType>();
        foreach (var type in moduleType.Types)
        {
            var module = (IModule)ActivatorUtilities.CreateInstance(app.Services, type);
            var url = $"/api";
            if (!string.IsNullOrEmpty(module.EndpointPrefix))
            {
                url += $"/{module.EndpointPrefix}";
            }
            var group = app.MapGroup(url).WithTags(module.EndpointPrefix);

            group.MapGet("/ping", () => $"{module.EndpointPrefix} pong!").WithTags("ping module api");

            var assemblyModule = type.Assembly;

            var endpointTypes = assemblyModule.GetTypes()
                .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var endpointType in endpointTypes)
            {
                var endpointInstance = (IEndpoint)Activator.CreateInstance(endpointType)!;
                endpointInstance.MapEndpoint(group);
            }
        }
    }

    private static List<Assembly> LoadAssemblies(string? rootPath)
    {
        var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());

        if (!string.IsNullOrEmpty(rootPath))
        {
            foreach (var dll in Directory.EnumerateFiles(rootPath, "*.dll"))
            {
                try
                {
                    var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(dll));
                    assemblies.Add(asm);
                }
                catch { /* ignore load errors */ }
            }
        }

        return assemblies.Distinct().ToList();
    }

    private static List<Type> FindModuleTypes(List<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch { return []; }
            })
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .ToList();
    }

    private static void ConfigureModuleServices(WebApplicationBuilder builder, List<Type> moduleTypes)
    {
        foreach (var type in moduleTypes)
        {
            var module = (IModule)Activator.CreateInstance(type)!;
            var moduleAssembly = type.Assembly;
            var modulePath = Path.GetDirectoryName(moduleAssembly.Location)!;

            foreach (var moduleSettingFile in module.ModuleSettingFiles)
            {
                var jsonPath = Path.Combine(modulePath, moduleSettingFile);
                builder.Configuration.AddJsonFile(jsonPath, optional: false, reloadOnChange: true);
            }

            module.ConfigureServices(builder);
        }
    }

    private sealed class ModuleType
    {
        public List<Type> Types { get; }
        public ModuleType(List<Type> types) => Types = types;
    }
}
