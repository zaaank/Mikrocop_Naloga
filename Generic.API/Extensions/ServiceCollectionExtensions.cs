using Generic.Infrastructure.Extensions;

namespace Generic.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGenericServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Application services here
        
        // Add Infrastructure services
        services.AddInfrastructure(configuration);

        return services;
    }
}
