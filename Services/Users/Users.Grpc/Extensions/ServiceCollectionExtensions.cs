using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Users.Core.Database;
using Users.Core.Repositories;
using Users.Core.Repositories.Interfaces;

namespace Users.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();

        return serviceCollection;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        return serviceCollection;
    }

    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddEntityFrameworkSqlServer()
            .AddDbContext<UsersDbContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("MSSQL"), c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
            });

        return serviceCollection;
    }
}