using Microsoft.Extensions.DependencyInjection;
using Users.Core.Repositories;
using Users.Core.Repositories.Interfaces;

namespace Users.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();

        return serviceCollection;
    }
}