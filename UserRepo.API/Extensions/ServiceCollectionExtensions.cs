using UserRepo.Infrastructure.Extensions;

namespace UserRepo.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserRepoServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Application services here
        
        // Add Infrastructure services
        services.AddInfrastructure(configuration);

        return services;
    }
}

