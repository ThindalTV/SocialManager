using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SocialManager;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add service discovery for Aspire
builder.Services.AddServiceDiscovery();

// Configure HttpClient for SocialManagerApi with service discovery
builder.Services.AddHttpClient("SocialManagerApiClient", client =>
{
    // The service name matches the one defined in the Aspire Host
    client.BaseAddress = new Uri("https+http://SocialManagerApi");
})
.AddServiceDiscovery();

// Add a default HttpClient for backward compatibility (optional)
builder.Services.AddScoped(sp => 
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("SocialManagerApiClient"));

await builder.Build().RunAsync();
