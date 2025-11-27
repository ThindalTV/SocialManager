using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialManager.SocialProvider.BlueSky.Client;
using SocialManager.SocialProvider.BlueSky.Configuration;
using SocialProvider;

namespace SocialManager.SocialProvider.BlueSky.Extensions;

/// <summary>
/// Extension methods for registering BlueSky Provider services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds BlueSky provider services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="parentSection">Optional parent section name (e.g., "SocialProviders" for nested config).</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddBlueSkyProvider(
        this IServiceCollection services,
        IConfiguration configuration,
        string? parentSection = null)
    {
        var sectionPath = string.IsNullOrEmpty(parentSection)
            ? BlueSkyProviderConfiguration.SectionName
            : $"{parentSection}:{BlueSkyProviderConfiguration.SectionName}";

        services.AddOptions<BlueSkyProviderConfiguration>()
            .Bind(configuration.GetSection(sectionPath));

        services.AddSingleton<IBlueSkyApiClient, FishyFlipApiClient>();
        services.AddSingleton<ISocialProvider, BlueSkyProvider>();

        return services;
    }

    /// <summary>
    /// Adds BlueSky provider services with explicit configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure the options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddBlueSkyProvider(
        this IServiceCollection services,
        Action<BlueSkyProviderConfiguration> configureOptions)
    {
        services.Configure(configureOptions);

        services.AddSingleton<IBlueSkyApiClient, FishyFlipApiClient>();
        services.AddSingleton<ISocialProvider, BlueSkyProvider>();

        return services;
    }
}
