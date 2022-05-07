using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NsfwDetectionPb;
using Videos.Core.GrpcServices;

namespace Videos.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNsfwGrpc(this IServiceCollection serviceCollection, string grpcUrl)
    {
        serviceCollection.AddGrpcClient<NsfwDetectionProtoService.NsfwDetectionProtoServiceClient>
            (client => client.Address = new Uri(grpcUrl));
        
        serviceCollection.AddScoped<NsfwDetectionGrpcService>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddMediatr(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        
        return serviceCollection;
    }
}