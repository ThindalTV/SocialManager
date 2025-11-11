using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace SocialManager.Admin;

public static class SocialManagerAdminExtensions
{
    /// <summary>
    /// Adds SocialManager Admin services to the service collection.
    /// </summary>
    public static IServiceCollection AddSocialManagerAdmin(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();

        return services;
    }

    /// <summary>
    /// Maps the SocialManager Admin Blazor WebAssembly application to the /admin path.
    /// </summary>
    public static IEndpointRouteBuilder UseSocialManagerAdmin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(App).Assembly);

        return endpoints;
    }
}
