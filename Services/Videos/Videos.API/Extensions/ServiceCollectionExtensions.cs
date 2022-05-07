using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Videos.Core.Database;

namespace Videos.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHealthChecks()
            .AddCheck("Videos API", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<VideosDbContext>("Videos MSSQL Server");
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddEntityFrameworkSqlServer()
            .AddDbContext<VideosDbContext>(o => {
                o.UseSqlServer(configuration.GetConnectionString("SqlServer"), c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
            });

        return serviceCollection;
    }
}