using Microsoft.Extensions.Diagnostics.HealthChecks;
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
}