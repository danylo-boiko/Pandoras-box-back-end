﻿namespace Auth.API.Extensions
{
    using Core.Configurations;
    using Core.Database;
    using Core.Database.Entities.Identity;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public static class ServiceCollectionExtensions
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
                .AddDbContext<BaseDbContext>(o => {
                    o.UseSqlServer(configuration.GetConnectionString("SqlServer"), c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
                });

            return serviceCollection;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddIdentity<ScamUser, ScamRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<BaseDbContext>();

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
            return serviceCollection;
        }

        public static IServiceCollection AddMediatr(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(Program).Assembly);
            return serviceCollection;
        }
    }
}