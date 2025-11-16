using Aspire.Microsoft.EntityFrameworkCore.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialManager.Data.CosmosDb.Repositories;
using SocialManager.Data.Repositories;

namespace SocialManager.Data.CosmosDb;

public static class RegisterCosmosDbExtensions
{
    public static IServiceCollection AddCosmosDb(this IServiceCollection services, IHostApplicationBuilder builder)
    {
        // Register DbContext with Aspire connection string
        builder.AddCosmosDbContext<SocialManagerDbContext>("cosmosdb", "SocialManagerStorage");

        services.AddTransient<IUnitOfWork, CosmosDbUnitOfWork>();

        // Register Repositories
        services.AddTransient<IEntryRepository, CosmosDbEntryRepository>();

        return services;
    }
}
