using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tags.Core.Repositories;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddMediatr(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITagsRepository, TagsRepository>();

        return serviceCollection;
    }
}