using SocialManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add SocialManager Admin Blazor WebAssembly
builder.Services.AddSocialManagerAdmin();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

// Rewrite /admin/_content to /_content for static assets from packages
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    if (path != null && path.StartsWith("/admin/_content/", StringComparison.OrdinalIgnoreCase))
    {
        context.Request.Path = path.Substring(6); // Remove "/admin" prefix
    }
    await next();
});

// Serve Blazor framework files from /admin/_framework
app.UseBlazorFrameworkFiles("/admin");

// Serve static files (including _content from referenced libraries)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAntiforgery();

// Enable WebAssembly debugging for the /admin path
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.MapRazorPages();
app.MapStaticAssets();

// Map the admin fallback route
app.MapFallbackToFile("/admin/{*path:nonfile}", "admin/index.html");

app.Run();
