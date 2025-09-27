var builder = WebApplication.CreateBuilder(args);
builder.AddHostConfigureServices();
var app = builder.Build();
app.UseHostConfigure();
await app.RunAsync();