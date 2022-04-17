using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Users.Core.Configurations;
using Users.Core.Database;
using Users.Core.Database.Entities.Identity;
using Users.Core.Repositories;
using Users.Core.Repositories.Interfaces;
using Users.Core.Services.Email;
using Users.Core.Services.User;

namespace Users.API.Extensions;

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
                .AddDbContext<UsersDbContext>(o => {
                    o.UseSqlServer(configuration.GetConnectionString("MSSQL"), c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
                });

            return serviceCollection;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddIdentity<ScamUser, ScamRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<UsersDbContext>();

            serviceCollection.Configure<IdentityOptions>(o =>
            {
                o.User.RequireUniqueEmail = true;

                o.Password.RequiredLength = 8;
                o.Password.RequireNonAlphanumeric = false;

                o.SignIn.RequireConfirmedEmail = true;
            });

            return serviceCollection;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.Configure<GoogleSmtpCredentials>(configuration.GetSection(nameof(GoogleSmtpCredentials)));
            serviceCollection.Configure<StorageServiceOptions>(configuration.GetSection(nameof(StorageServiceOptions)));
            
            return serviceCollection;
        }
        
        public static IServiceCollection AddCustomServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEmailService, EmailService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            
            return serviceCollection;
        }
        
        public static IServiceCollection AddHealthCheck(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHealthChecks()
                .AddCheck("Users API", () => HealthCheckResult.Healthy())
                .AddDbContextCheck<UsersDbContext>("Users MSSQL Server");
            
            return serviceCollection;
        }
}