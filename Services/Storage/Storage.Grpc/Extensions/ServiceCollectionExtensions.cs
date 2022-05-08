using Calzolari.Grpc.AspNetCore.Validation;
using EventBus.Messages.Consts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Storage.Core.Database;
using Storage.Core.Repositories.StorageItem;
using Storage.Core.Repositories.UserStorageItem;
using Storage.Grpc.EventBusConsumers;
using Storage.Grpc.Validation;

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
                .AddDbContext<StorageDbContext>(o =>
                {
                    o.UseSqlServer(configuration.GetConnectionString("MSSQL"),
                        c => c.MigrationsAssembly(typeof(Program).Assembly.FullName));
                });

            return serviceCollection;
        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMassTransit(config =>
            {
                config.AddConsumer<MediaFileDeleteConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["EventBusSettings:HostAddress"]);
                    
                    cfg.ReceiveEndpoint(EventBusConstants.MediaFilesDeletingQueue, c =>
                    {
                        c.ConfigureConsumer<MediaFileDeleteConsumer>(ctx);
                    });
                });
            });
            
            return serviceCollection;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IStorageItemRepository, StorageItemRepository>();
            services.AddScoped<IUserStorageItemRepository, UserStorageItemRepository>();

            return services;
        }
        
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidator<SaveMediaFilesRequestValidator>();

            services.AddGrpcValidation();
            
            return services;
        }

        public static IServiceCollection ConfigureCustomSettings(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<FileHashingSettings>(configuration.GetSection("FileHashingOptions"));

            return services;
        }
    }
}