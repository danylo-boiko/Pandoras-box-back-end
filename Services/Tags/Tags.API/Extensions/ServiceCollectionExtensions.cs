using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Tags.Core.CQRS.Commands.CreateTag;
using Tags.Core.CQRS.Commands.UpdateTag;
using Tags.Core.CQRS.Queries.GetTagsByPattern;
using Tags.Core.Database;

namespace Tags.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHealthChecks()
            .AddCheck("Tags API", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<TagsDbContext>("Tags MSSQL Server");
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddFluentValidationValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssemblyContaining<CreateTagCommandValidator>();
        serviceCollection.AddValidatorsFromAssemblyContaining<UpdateTagCommandValidator>();
        serviceCollection.AddValidatorsFromAssemblyContaining<GetTagsByPatternQueryValidator>();

        serviceCollection.AddFluentValidation();
        
        return serviceCollection;
    }
}