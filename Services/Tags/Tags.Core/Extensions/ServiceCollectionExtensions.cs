using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tags.Core.Behaviours;
using Tags.Core.Database;
using Tags.Core.GrpcServices;
using Tags.Core.Protos;
using Tags.Core.Repositories;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddEntityFrameworkSqlServer()
            .AddDbContext<TagsDbContext>(o => {
                o.UseSqlServer(configuration.GetConnectionString("SqlServer"), c => c.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            });

        return serviceCollection;
    }
    
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
    
    public static IServiceCollection AddUsersGrpcServer(this IServiceCollection serviceCollection, string grpcUrl)
    {
        serviceCollection.AddGrpcClient<UsersProtoService.UsersProtoServiceClient>
            (client => client.Address = new Uri(grpcUrl));
        
        serviceCollection.AddScoped<UsersGrpcService>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddHealthCheck(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHealthChecks().AddDbContextCheck<TagsDbContext>();
        
        return serviceCollection;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return serviceCollection;
    }
}