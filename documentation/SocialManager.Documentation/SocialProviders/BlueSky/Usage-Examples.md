# BlueSky Provider Usage Examples

## Overview
This guide provides practical examples for using the BlueSky provider in your application.

## Configuration Structure Options

The BlueSky Provider supports both **flat** and **nested** configuration structures.

## Basic Setup

### Option 1: Flat Configuration (Default)

#### appsettings.json
```json
{
  "BlueSkyProvider": {
    "Active": true,
    "Platform": "BlueSky",
    "Identifier": "alice.bsky.social",
    "AppPassword": "xxxx-xxxx-xxxx-xxxx",
    "PdsUrl": "https://bsky.social"
  }
}
```

#### Program.cs
```csharp
using SocialManager.SocialProvider.BlueSky.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register BlueSky Provider with flat configuration
builder.Services.AddBlueSkyProvider(builder.Configuration);

var app = builder.Build();
app.Run();
```

### Option 2: Nested Configuration (Recommended for Multiple Providers)

#### appsettings.json
```json
{
  "SocialProviders": {
    "BlueSkyProvider": {
      "Active": true,
      "Platform": "BlueSky",
      "Identifier": "alice.bsky.social",
      "AppPassword": "xxxx-xxxx-xxxx-xxxx",
      "PdsUrl": "https://bsky.social"
    }
  }
}
```

#### Program.cs
```csharp
using SocialManager.SocialProvider.BlueSky.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register BlueSky Provider with nested configuration
builder.Services.AddBlueSkyProvider(builder.Configuration, "SocialProviders");

var app = builder.Build();
app.Run();
```

### Option 3: Multiple Providers with Nested Structure

#### appsettings.json
```json
{
  "SocialProviders": {
    "BlueSkyProvider": {
      "Active": true,
      "Identifier": "alice.bsky.social",
      "AppPassword": "xxxx-xxxx-xxxx-xxxx"
    },
    "XProvider": {
      "Active": true,
      "ApiKey": "x-api-key",
      "ApiSecret": "x-api-secret"
    }
  }
}
```

#### Program.cs
```csharp
using SocialManager.SocialProvider.BlueSky.Extensions;
using SocialManager.SocialProvider.X.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register all providers with the same parent section
builder.Services.AddBlueSkyProvider(builder.Configuration, "SocialProviders");
builder.Services.AddXProvider(builder.Configuration, "SocialProviders");

var app = builder.Build();
app.Run();
```

## Inject and Use

```csharp
using SocialProvider;
using SocialManager.Data.Types.Social;

public class SocialMediaService
{
    private readonly ISocialProvider _blueSkyProvider;
    private readonly ILogger<SocialMediaService> _logger;

    public SocialMediaService(
        ISocialProvider blueSkyProvider,
        ILogger<SocialMediaService> logger)
    {
        _blueSkyProvider = blueSkyProvider;
        _logger = logger;
    }

    public async Task PostToBlueSkyAsync(string content, CancellationToken ct = default)
    {
        var post = new Post
        {
            Platform = "BlueSky",
            Content = content,
            MediaUrl = null,
            LinkUrl = null
        };

        await _blueSkyProvider.Post(post, ct);
        _logger.LogInformation("Posted to BlueSky successfully");
    }
}
```

## Posting Examples

### Simple Text Post

```csharp
var post = new Post
{
    Platform = "BlueSky",
    Content = "Hello from SocialManager! ??? #dotnet #csharp",
    MediaUrl = null,
    LinkUrl = null
};

await provider.Post(post, CancellationToken.None);
```

### Post with Link

```csharp
var post = new Post
{
    Platform = "BlueSky",
    Content = "Check out this amazing article!",
    LinkUrl = "https://example.com/article",
    MediaUrl = null
};

await provider.Post(post, CancellationToken.None);
// Result: "Check out this amazing article!\n\nhttps://example.com/article"
```

### Post with Image (URL)

```csharp
var post = new Post
{
    Platform = "BlueSky",
    Content = "Beautiful sunset today! ??",
    MediaUrl = "https://example.com/images/sunset.jpg",
    LinkUrl = null
};

await provider.Post(post, CancellationToken.None);
```

### Post with Image (Local File)

```csharp
var post = new Post
{
    Platform = "BlueSky",
    Content = "My latest creation! ??",
    MediaUrl = @"C:\Images\artwork.jpg",
    LinkUrl = null
};

await provider.Post(post, CancellationToken.None);
```

## Retrieving Statistics

### Get Post Statistics

```csharp
// BlueSky uses AT URIs in the format: at://did:plc:xxx/app.bsky.feed.post/xxx
var postUri = "at://did:plc:abc123/app.bsky.feed.post/xyz789";

var stats = await provider.GetStatistics(postUri, CancellationToken.None);

Console.WriteLine($"Post: {stats.Title}");
Console.WriteLine($"Platform: {stats.Platform}");
Console.WriteLine($"Likes: {stats.Likes}");
Console.WriteLine($"Reposts + Quotes: {stats.Shares}");
Console.WriteLine($"Replies: {stats.Comments}");
Console.WriteLine($"Retrieved at: {stats.RetrievedAt:u}");
```

## Error Handling

### Basic Error Handling

```csharp
using SocialProvider.Exceptions;

try
{
    await provider.Post(post, ct);
}
catch (SocialProviderAuthenticationException ex)
{
    _logger.LogError(ex, "Authentication failed. Check credentials.");
}
catch (SocialProviderRateLimitException ex)
{
    _logger.LogWarning(ex, "Rate limit exceeded. Retry at: {ResetTime}", ex.ResetTime);
}
catch (SocialProviderException ex)
{
    _logger.LogError(ex, "Provider error on {Platform}: {Message}", ex.Platform, ex.Message);
}
```

### Handling Rate Limits

```csharp
try
{
    await provider.Post(post, ct);
}
catch (SocialProviderRateLimitException ex)
{
    if (ex.ResetTime.HasValue)
    {
        var delay = ex.ResetTime.Value - DateTime.UtcNow;
        _logger.LogInformation("Waiting {Seconds} seconds for rate limit reset", delay.TotalSeconds);
        
        await Task.Delay(delay, ct);
        
        // Retry
        await provider.Post(post, ct);
    }
}
```

## Advanced Usage

### Multiple Social Providers

If you have multiple social providers, use the collection:

```csharp
public class SocialMediaService
{
    private readonly IEnumerable<ISocialProvider> _providers;

    public SocialMediaService(IEnumerable<ISocialProvider> providers)
    {
        _providers = providers;
    }

    public async Task PostToAllPlatformsAsync(Post post)
    {
        var tasks = _providers.Select(p => p.Post(post, CancellationToken.None));
        await Task.WhenAll(tasks);
    }
}
```

### Conditional Posting Based on Configuration

```csharp
using SocialManager.SocialProvider.BlueSky.Configuration;
using Microsoft.Extensions.Options;

public class BlueSkyPostingService
{
    private readonly ISocialProvider _blueSkyProvider;
    private readonly BlueSkyProviderConfiguration _config;
    private readonly ILogger<BlueSkyPostingService> _logger;

    public BlueSkyPostingService(
        ISocialProvider blueSkyProvider,
        IOptions<BlueSkyProviderConfiguration> config,
        ILogger<BlueSkyPostingService> logger)
    {
        _blueSkyProvider = blueSkyProvider;
        _config = config.Value;
        _logger = logger;
    }

    public async Task PostIfActiveAsync(Post post)
    {
        if (_config.Active)
        {
            await _blueSkyProvider.Post(post, CancellationToken.None);
            _logger.LogInformation("Posted to BlueSky successfully");
        }
        else
        {
            _logger.LogInformation("BlueSky Provider is disabled, skipping post");
        }
    }
}
```

### Validation Before Posting

```csharp
public async Task ValidateAndPostAsync(string content)
{
    const int MaxLength = 300;
    
    if (string.IsNullOrWhiteSpace(content))
    {
        throw new ArgumentException("Content cannot be empty");
    }
    
    if (content.Length > MaxLength)
    {
        throw new ArgumentException($"Content exceeds {MaxLength} characters");
    }
    
    var post = new Post
    {
        Platform = "BlueSky",
        Content = content,
        MediaUrl = null,
        LinkUrl = null
    };
    
    await _blueSkyProvider.Post(post, CancellationToken.None);
}
```

## Testing Authentication

```csharp
using SocialManager.SocialProvider.BlueSky.Client;

public class BlueSkyAuthenticationService
{
    private readonly IBlueSkyApiClient _client;

    public BlueSkyAuthenticationService(IBlueSkyApiClient client)
    {
        _client = client;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var user = await _client.GetAuthenticatedUserAsync(CancellationToken.None);
            Console.WriteLine($"? Connected as: @{user.Handle}");
            Console.WriteLine($"   Display Name: {user.DisplayName}");
            Console.WriteLine($"   Followers: {user.FollowersCount}");
            Console.WriteLine($"   Posts: {user.PostsCount}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Connection failed: {ex.Message}");
            return false;
        }
    }
}
```

## Minimal API Example

```csharp
using SocialManager.Data.Types.Social;
using SocialProvider;

app.MapPost("/api/social/post-to-bluesky", async (
    PostRequest request,
    ISocialProvider provider,
    CancellationToken ct) =>
{
    var post = new Post
    {
        Platform = "BlueSky",
        Content = request.Content,
        MediaUrl = request.MediaUrl,
        LinkUrl = request.LinkUrl
    };

    await provider.Post(post, ct);

    return Results.Ok(new { message = "Posted successfully" });
});

app.MapGet("/api/social/statistics/{postUri}", async (
    string postUri,
    ISocialProvider provider,
    CancellationToken ct) =>
{
    // URL decode the AT URI
    var decodedUri = Uri.UnescapeDataString(postUri);
    var stats = await provider.GetStatistics(decodedUri, ct);
    return Results.Ok(stats);
});

record PostRequest(string Content, string? MediaUrl, string? LinkUrl);
```

## Blazor Component Example

```razor
@page "/post-to-bluesky"
@inject ISocialProvider BlueSkyProvider
@inject ILogger<PostToBlueSky> Logger

<div class="post-form">
    <h3>Post to BlueSky</h3>
    
    <textarea @bind="content" 
              placeholder="What's on your mind? (300 characters max)" 
              maxlength="300"
              rows="5"></textarea>
    
    <div class="character-count">
        @content.Length / 300
    </div>
    
    <button @onclick="PostAsync" disabled="@isPosting">
        @(isPosting ? "Posting..." : "Post to BlueSky ???")
    </button>
    
    @if (!string.IsNullOrEmpty(successMessage))
    {
        <div class="success">? @successMessage</div>
    }
    
    @if (!string.IsNullOrEmpty(errorMessage))
    {
        <div class="error">? @errorMessage</div>
    }
</div>

@code {
    private string content = "";
    private bool isPosting = false;
    private string? successMessage = null;
    private string? errorMessage = null;

    private async Task PostAsync()
    {
        isPosting = true;
        errorMessage = null;
        successMessage = null;

        try
        {
            var post = new Post
            {
                Platform = "BlueSky",
                Content = content,
                MediaUrl = null,
                LinkUrl = null
            };

            await BlueSkyProvider.Post(post, CancellationToken.None);
            
            successMessage = "Posted to BlueSky successfully!";
            content = "";
            Logger.LogInformation("Posted to BlueSky successfully");
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            Logger.LogError(ex, "Failed to post to BlueSky");
        }
        finally
        {
            isPosting = false;
        }
    }
}
```

```css
.post-form {
    max-width: 600px;
    margin: 20px auto;
    padding: 20px;
}

.post-form textarea {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
    resize: vertical;
}

.character-count {
    text-align: right;
    color: #666;
    margin: 5px 0;
}

.post-form button {
    padding: 10px 20px;
    background-color: #1185fe;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.post-form button:disabled {
    background-color: #ccc;
    cursor: not-allowed;
}

.success {
    color: green;
    margin-top: 10px;
}

.error {
    color: red;
    margin-top: 10px;
}
```

## Background Job Example

```csharp
using Hangfire;

public class ScheduledPostService
{
    private readonly ISocialProvider _provider;
    private readonly ILogger<ScheduledPostService> _logger;

    public ScheduledPostService(
        ISocialProvider provider,
        ILogger<ScheduledPostService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public void SchedulePost(string content, DateTime scheduledTime)
    {
        BackgroundJob.Schedule(
            () => PostToBlueSkyAsync(content),
            scheduledTime);
    }

    public async Task PostToBlueSkyAsync(string content)
    {
        try
        {
            var post = new Post
            {
                Platform = "BlueSky",
                Content = content,
                MediaUrl = null,
                LinkUrl = null
            };

            await _provider.Post(post, CancellationToken.None);
            _logger.LogInformation("Scheduled post published successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish scheduled post");
            throw;
        }
    }
}
```

## Working with AT URIs

BlueSky uses AT URIs for identifying posts:

```csharp
// AT URI format: at://did:plc:xxx/app.bsky.feed.post/xxx
var atUri = "at://did:plc:abc123xyz/app.bsky.feed.post/3klmnopqrst";

// When passing in URLs (e.g., from API), encode them
var encodedUri = Uri.EscapeDataString(atUri);

// When receiving from provider, they're already in correct format
var stats = await provider.GetStatistics(atUri, CancellationToken.None);
```

## See Also

- [Configuration.md](Configuration.md) - Configuration options (flat vs nested)
- [API-Setup.md](API-Setup.md) - Getting BlueSky credentials
- [Troubleshooting.md](Troubleshooting.md) - Common issues and solutions
