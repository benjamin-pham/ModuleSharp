using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Contract.Infrastructure.Database;

public static class Postgres
{
    public static Action<IServiceProvider, DbContextOptionsBuilder> StandardOptions(IConfiguration configuration) =>
        (serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database")!)
            .UseSnakeCaseNamingConvention();
        };
}
