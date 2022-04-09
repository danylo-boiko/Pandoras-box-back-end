using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Storage.Core.Database;

namespace Storage.Grpc.Extensions
{
    using Core.Services;
    using Core.Settings;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddEntityFrameworkSqlServer()
                .AddDbContext<StorageDbContext>(o => {
                    o.UseSqlServer(configuration.GetConnectionString("MSSQL"), c => c.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
                });

            return serviceCollection;
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();

            return services;
        }

        public static IServiceCollection ConfigureCustomSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileHashingSettings>(configuration.GetSection("FileHashingOptions"));

            return services;
        }
    }
}
