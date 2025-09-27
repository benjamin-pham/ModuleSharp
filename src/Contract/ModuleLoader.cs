using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.Loader;

namespace Contract;

public static class ModuleLoader
{
    public static void AddHostConfigureServices(this WebApplicationBuilder builder, string? rootPath = null)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true; // optional
        });

        builder.Services.AddControllers(options =>
        {
            options.Conventions.Insert(0, new GlobalRoutePrefixConvention("api/module"));
        });

        builder.AddModuleServices(rootPath);
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
        MapEndpoints(app);
        app.MapFallback(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Resource not found");
        });
    }

    private static void AddModuleServices(
        this WebApplicationBuilder builder,
        string? rootPath = null)
    {
        var assemblies = new List<Assembly>();
        assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());

        if (!string.IsNullOrEmpty(rootPath))
        {
            foreach (var dll in Directory.EnumerateFiles(rootPath, "*.dll"))
            {
                try
                {
                    var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(dll));
                    assemblies.Add(asm);
                }
                catch { }
            }
        }

        var moduleTypes = assemblies
            .Distinct()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch { return []; }
            })
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .ToList();

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

        builder.Services.AddSingleton(new ModuleType(moduleTypes));
    }

    private static IEndpointRouteBuilder MapEndpoints(this WebApplication app)
    {
        var moduleType = app.Services.GetRequiredService<ModuleType>();
        foreach (var type in moduleType.Types)
        {
            var module = (IModule)ActivatorUtilities.CreateInstance(app.Services, type);
            var group = app.MapGroup($"/api/{module.EndpointPrefix}").WithTags(module.EndpointPrefix);
            group.MapGet("/ping", () => $"{module.EndpointPrefix} pong!");
            module.ConfigureEndpoints(group);
        }
        return app;
    }

    private sealed class ModuleType(List<Type> types)
    {
        public List<Type> Types { get; private set; } = types;
    }
}