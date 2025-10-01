using Microsoft.AspNetCore.Builder;

namespace Contract;

public static class Startup
{
    public static async Task Run(string[] args)
    {
        string? rootPath = args.FirstOrDefault();
        var builder = WebApplication.CreateBuilder(args);
        builder.AddHostConfigureServices(rootPath: rootPath);
        var app = builder.Build();
        app.UseHostConfigure();
        await app.RunAsync();
    }
}
