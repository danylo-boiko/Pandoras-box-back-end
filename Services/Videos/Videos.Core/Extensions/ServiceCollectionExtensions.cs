﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NsfwDetectionPb;
using Tags.Grpc.Protos;
using Users.Grpc.Protos;
using Videos.Core.GrpcServices;

namespace Videos.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageGrpc(this IServiceCollection serviceCollection, string grpcUrl)
    {
        serviceCollection.AddGrpcClient<Storage.Grpc.Storage.StorageClient>
            (client => client.Address = new Uri(grpcUrl));
        
        serviceCollection.AddScoped<StorageGrpcService>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddUsersGrpc(this IServiceCollection serviceCollection, string grpcUrl)
    {
        serviceCollection.AddGrpcClient<UsersProtoService.UsersProtoServiceClient>
            (client => client.Address = new Uri(grpcUrl));
        
        serviceCollection.AddScoped<UsersGrpcService>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddTagsGrpc(this IServiceCollection serviceCollection, string grpcUrl)
    {
        serviceCollection.AddGrpcClient<TagsProtoService.TagsProtoServiceClient>
            (client => client.Address = new Uri(grpcUrl));
        
        serviceCollection.AddScoped<TagsGrpcService>();

        return serviceCollection;
    }
    
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