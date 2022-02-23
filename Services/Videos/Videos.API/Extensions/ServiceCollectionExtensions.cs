using Microsoft.Extensions.Diagnostics.HealthChecks;
using NsfwDetectionPb;
using Videos.API.GrpcServices;

namespace Videos.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNsfwGrpc(this IServiceCollection serviceCollection, string grpcUrl)
    {
        serviceCollection.AddGrpcClient<NsfwDetectionProtoService.NsfwDetectionProtoServiceClient>
            (client => client.Address = new Uri(grpcUrl));
        
        serviceCollection.AddScoped<NsfwDetectionGrpcService>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddHealthCheck(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHealthChecks()
            .AddCheck("Videos API", () => HealthCheckResult.Healthy());
        
        return serviceCollection;
    }
}