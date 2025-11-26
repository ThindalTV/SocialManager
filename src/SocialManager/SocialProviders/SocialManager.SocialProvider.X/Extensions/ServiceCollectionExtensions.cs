using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SocialManager.SocialProvider.X.Client;
using SocialManager.SocialProvider.X.Configuration;
using SocialProvider;

namespace SocialManager.SocialProvider.X.Extensions;

/// <summary>
/// Extension methods for registering X Provider services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds X (Twitter) provider services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="parentSection">Optional parent section name (e.g., "SocialProviders" for nested config).</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddXProvider(
        this IServiceCollection services,
        IConfiguration configuration,
        string? parentSection = null)
    {
        var sectionPath = string.IsNullOrEmpty(parentSection)
            ? XProviderConfiguration.SectionName
            : $"{parentSection}:{XProviderConfiguration.SectionName}";

        services.AddOptions<XProviderConfiguration>()
            .Bind(configuration.GetSection(sectionPath));

        services.AddSingleton<IXApiClient, LinqToTwitterApiClient>();
        services.AddSingleton<ISocialProvider, XProvider>();

        return services;
    }

    /// <summary>
    /// Adds X (Twitter) provider services with explicit configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure the options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddXProvider(
        this IServiceCollection services,
        Action<XProviderConfiguration> configureOptions)
    {
        services.Configure(configureOptions);

        services.AddSingleton<IXApiClient, LinqToTwitterApiClient>();
        services.AddSingleton<ISocialProvider, XProvider>();

        return services;
    }
}
