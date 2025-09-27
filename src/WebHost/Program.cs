var builder = WebApplication.CreateBuilder(args);
builder.AddHostConfigureServices(rootPath: AppContext.BaseDirectory);
var app = builder.Build();
app.UseHostConfigure();
await app.RunAsync();