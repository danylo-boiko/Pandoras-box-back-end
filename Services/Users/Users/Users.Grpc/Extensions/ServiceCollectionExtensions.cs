using Microsoft.EntityFrameworkCore;
using Users.Core.Database;

namespace Users.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddEntityFrameworkSqlServer()
            .AddDbContext<BaseDbContext>(o => {
                o.UseSqlServer(configuration.GetConnectionString("SqlServer"), c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
            });

        return serviceCollection;
    }
}