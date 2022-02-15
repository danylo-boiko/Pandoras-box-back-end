using Microsoft.EntityFrameworkCore;
using Tags.Core.Database;

namespace Tags.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddEntityFrameworkSqlServer()
            .AddDbContext<TagsDbContext>(o => {
                o.UseSqlServer(configuration.GetConnectionString("SqlServer"), c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
            });

        return serviceCollection;
    }
}