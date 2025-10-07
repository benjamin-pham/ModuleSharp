using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.Loader;

namespace Contract.ModuleRegister;

public static class ModuleLoader
{
    public static void AddHostConfigureServices(this WebApplicationBuilder builder)
    {
        var moduleManager = FindModules();

        builder.Services.AddSingleton(moduleManager);

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

        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new ModuleRoutePrefixConvention(moduleManager));
        });

        builder.AddModuleServices(moduleManager);
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

    private static void AddModuleServices(this WebApplicationBuilder builder, ModuleManager moduleManager)
    {
        ConfigureModuleServices(builder, moduleManager);

        builder.Services.AddApplicationDbContexts(builder.Configuration, moduleManager);

        builder.Services.AddWithAttributes(moduleManager);
    }

    private static void MapModuleEndpoints(this WebApplication app)
    {
        var moduleManager = app.Services.GetRequiredService<ModuleManager>();
        foreach (var module in moduleManager.AppModules)
        {
            var url = $"/api";
            if (!string.IsNullOrEmpty(module.Instance.EndpointPrefix))
            {
                url += $"/{module.Instance.EndpointPrefix}";
            }
            var group = app.MapGroup(url).WithTags(module.Instance.EndpointPrefix);

            group.MapGet("/ping", () => $"{module.Instance.EndpointPrefix} pong!").WithTags("ping module api");

            var endpointTypes = module.AssemblyTypes
                .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var endpointType in endpointTypes)
            {
                var endpointInstance = (IEndpoint)Activator.CreateInstance(endpointType)!;
                endpointInstance.MapEndpoint(group);
            }
        }
    }

    private static List<Assembly> LoadAssemblies()
    {
        var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());

        if (!string.IsNullOrEmpty(AppContext.BaseDirectory))
        {
            foreach (var dll in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.dll"))
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

    private static ModuleManager FindModules()
    {
        var assemblies = LoadAssemblies();

        var modules = assemblies
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch { return []; }
            })
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .Select(t =>
            {
                var instance = (IModule)Activator.CreateInstance(t)!;
                return new ModuleManager.AppModule(t.Assembly, instance);
            })
            .ToList();

        return new(modules);
    }

    private static void ConfigureModuleServices(WebApplicationBuilder builder, ModuleManager moduleManager)
    {
        foreach (var module in moduleManager.AppModules)
        {
            var modulePath = Path.GetDirectoryName(module.Assembly.Location)!;

            foreach (var moduleSettingFile in module.Instance.ModuleSettingFiles)
            {
                var jsonPath = Path.Combine(modulePath, moduleSettingFile);
                builder.Configuration.AddJsonFile(jsonPath, optional: false, reloadOnChange: true);
            }

            module.Instance.ConfigureServices(builder);
        }
    }
}
