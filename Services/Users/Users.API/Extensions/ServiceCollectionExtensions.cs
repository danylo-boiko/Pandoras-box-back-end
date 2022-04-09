using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Users.Core.Database;

namespace Users.API.Extensions
{
    using Core.Configurations;
    using Core.Database.Entities.Identity;
    using Core.Services.Email;
    using Core.Services.User;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Identity;

    internal static class ServiceCollectionExtensions
    {
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
}
