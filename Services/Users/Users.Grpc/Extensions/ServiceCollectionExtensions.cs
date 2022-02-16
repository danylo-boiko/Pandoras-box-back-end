using Microsoft.EntityFrameworkCore;
using Users.Core.Database;
using Users.Core.Repositories;
using Users.Core.Repositories.Interfaces;

namespace Users.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddEntityFrameworkSqlServer()
            .AddDbContext<UsersDbContext>(o => {
                o.UseSqlServer(configuration.GetConnectionString("SqlServer"), c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
            });

        return serviceCollection;
    }
    
    public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddHealthCheck(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHealthChecks();
        
        return serviceCollection;
    }

    public static IServiceCollection AddCustomRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();

        return serviceCollection;
    }
}