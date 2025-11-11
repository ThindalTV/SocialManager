using SocialManager.Admin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add SocialManager Admin Blazor WebAssembly
builder.Services.AddSocialManagerAdmin();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseWebAssemblyDebugging();
}

app.UseRouting();

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Map the Admin Blazor WebAssembly app at /admin
app.UseSocialManagerAdmin();

app.Run();
