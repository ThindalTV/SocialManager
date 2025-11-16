using Microsoft.Extensions.Hosting;
using SocialManager.Data.CosmosDb;

namespace SocialManager.API;

public static class SocialServicesExtensions
{
    public static IServiceCollection AddSocialManagerServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddCosmosDb(builder);

        return services;
    }
}
