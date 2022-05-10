using System.Reflection;
using EventBus.Messages.Consts;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tags.Grpc.Protos;
using Users.Grpc.Protos;
using Videos.Core.EventBusConsumers;
using Videos.Core.GrpcServices;

namespace Videos.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageGrpc(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddGrpcClient<Storage.Grpc.Storage.StorageClient>(client => 
        {
            client.Address = new Uri(configuration["GrpcServers:Storage"]);
        });
        
        serviceCollection.AddScoped<StorageGrpcService>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddUsersGrpc(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddGrpcClient<UsersProtoService.UsersProtoServiceClient>(client => 
        {
            client.Address = new Uri(configuration["GrpcServers:Users"]);
        });
        
        serviceCollection.AddScoped<UsersGrpcService>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddTagsGrpc(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddGrpcClient<TagsProtoService.TagsProtoServiceClient>(client => 
        {
            client.Address = new Uri(configuration["GrpcServers:Tags"]);
        });
        
        serviceCollection.AddScoped<TagsGrpcService>();

        return serviceCollection;
    }

    public static IServiceCollection AddMediatr(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddRabbitMQ(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMassTransit(config => 
        {
            config.AddConsumer<VideoClassificationStatusUpdateConsumer>();
            
            config.UsingRabbitMq((ctx, cfg) => 
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"]);
                
                cfg.ReceiveEndpoint(EventBusConstants.VideoClassificationStatusUpdatingQueue, c =>
                {
                    c.ConfigureConsumer<VideoClassificationStatusUpdateConsumer>(ctx);
                });
            });
        });

        return serviceCollection;
    }
}