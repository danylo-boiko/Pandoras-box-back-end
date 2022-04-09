using System.Reflection;

namespace Users.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return serviceCollection;
    }
}