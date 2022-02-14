namespace Storage.Grpc.Extensions
{
    using Core.Services;
    using Core.Settings;

    public static class ServiceCollectionExtensions
    {
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
