using Contract.ModuleRegister;
using Microsoft.AspNetCore.Builder;

namespace Contract;

public static class Startup
{
    public static async Task Run(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddHostConfigureServices();
        var app = builder.Build();
        app.UseHostConfigure();
        await app.RunAsync();
    }
}
