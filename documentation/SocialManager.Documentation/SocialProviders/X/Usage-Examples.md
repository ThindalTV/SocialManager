# X Provider Usage Examples

## Overview
This guide provides practical examples for using the X (Twitter) provider in your application.

## Configuration Structure Options

The X Provider supports both **flat** and **nested** configuration structures.

## Basic Setup

### Option 1: Flat Configuration (Default)

#### appsettings.json
```json
{
  "XProvider": {
    "Active": true,
    "Platform": "X",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret",
    "AccessToken": "your-access-token",
    "AccessTokenSecret": "your-access-token-secret"
  }
}
```

#### Program.cs
```csharp
using SocialManager.SocialProvider.X.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register X Provider with flat configuration
builder.Services.AddXProvider(builder.Configuration);

var app = builder.Build();
app.Run();
```

### Option 2: Nested Configuration (Recommended for Multiple Providers)

#### appsettings.json
```json
{
  "SocialProviders": {
    "XProvider": {
      "Active": true,
      "Platform": "X",
      "ApiKey": "your-api-key",
      "ApiSecret": "your-api-secret",
      "AccessToken": "your-access-token",
      "AccessTokenSecret": "your-access-token-secret"
    }
  }
}
```

#### Program.cs
```csharp
using SocialManager.SocialProvider.X.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register X Provider with nested configuration
builder.Services.AddXProvider(builder.Configuration, "SocialProviders");

var app = builder.Build();
app.Run();
```

### Option 3: Multiple Providers with Nested Structure

#### appsettings.json
```json
{
  "SocialProviders": {
    "XProvider": {
      "Active": true,
      "ApiKey": "x-api-key",
      "ApiSecret": "x-api-secret",
      "AccessToken": "x-access-token",
      "AccessTokenSecret": "x-access-token-secret"
    },
    "FacebookProvider": {
      "Active": true,
      "AppId": "facebook-app-id",
      "AppSecret": "facebook-app-secret"
    }
  }
}
```

#### Program.cs
```csharp
using SocialManager.SocialProvider.X.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register all providers with the same parent section
builder.Services.AddXProvider(builder.Configuration, "SocialProviders");
// Future: builder.Services.AddFacebookProvider(builder.Configuration, "SocialProviders");

var app = builder.Build();
app.Run();
```

### 1. Inject and Use

```csharp
using SocialProvider;
using SocialManager.Data.Types.Social;

public class SocialMediaService
{
    private readonly ISocialProvider _xProvider;
    private readonly ILogger<SocialMediaService> _logger;

    public SocialMediaService(
        ISocialProvider xProvider,
        ILogger<SocialMediaService> logger)
    {
        _xProvider = xProvider;
        _logger = logger;
    }

    public async Task PostToXAsync(string content, CancellationToken ct = default)
    {
        var post = new Post
        {
            Platform = "X",
            Content = content,
            MediaUrl = null,
            LinkUrl = null
        };

        await _xProvider.Post(post, ct);
        _logger.LogInformation("Posted to X successfully");
    }
}
```

## Posting Examples

### Simple Text Post

```csharp
var post = new Post
{
    Platform = "X",
    Content = "Hello from SocialManager! #dotnet #csharp",
    MediaUrl = null,
    LinkUrl = null
};

await provider.Post(post, CancellationToken.None);
```

### Post with Link

```csharp
var post = new Post
{
    Platform = "X",
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
    Platform = "X",
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
    Platform = "X",
    Content = "My latest creation! ??",
    MediaUrl = @"C:\Images\artwork.jpg",
    LinkUrl = null
};

await provider.Post(post, CancellationToken.None);
```

## Retrieving Statistics

### Get Tweet Statistics

```csharp
var tweetId = "1234567890"; // The tweet ID from X

var stats = await provider.GetStatistics(tweetId, CancellationToken.None);

Console.WriteLine($"Tweet: {stats.Title}");
Console.WriteLine($"Platform: {stats.Platform}");
Console.WriteLine($"Likes: {stats.Likes}");
Console.WriteLine($"Retweets: {stats.Shares}");
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

If you have multiple social providers, use named clients:

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
using SocialManager.SocialProvider.X.Configuration;
using Microsoft.Extensions.Options;

public class XPostingService
{
    private readonly ISocialProvider _xProvider;
    private readonly XProviderConfiguration _config;
    private readonly ILogger<XPostingService> _logger;

    public XPostingService(
        ISocialProvider xProvider,
        IOptions<XProviderConfiguration> config,
        ILogger<XPostingService> logger)
    {
        _xProvider = xProvider;
        _config = config.Value;
        _logger = logger;
    }

    public async Task PostIfActiveAsync(Post post)
    {
        if (_config.Active)
        {
            await _xProvider.Post(post, CancellationToken.None);
            _logger.LogInformation("Posted to X successfully");
        }
        else
        {
            _logger.LogInformation("X Provider is disabled, skipping post");
        }
    }
}
```

### Validation Before Posting

```csharp
public async Task ValidateAndPostAsync(string content)
{
    const int MaxLength = 280;
    
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
        Platform = "X",
        Content = content,
        MediaUrl = null,
        LinkUrl = null
    };
    
    await _xProvider.Post(post, CancellationToken.None);
}
```

## Testing Authentication

```csharp
using SocialManager.SocialProvider.X.Client;

public class XAuthenticationService
{
    private readonly IXApiClient _client;

    public XAuthenticationService(IXApiClient client)
    {
        _client = client;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var user = await _client.GetAuthenticatedUserAsync(CancellationToken.None);
            Console.WriteLine($"? Connected as: @{user.ScreenName}");
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

app.MapPost("/api/social/post-to-x", async (
    PostRequest request,
    ISocialProvider provider,
    CancellationToken ct) =>
{
    var post = new Post
    {
        Platform = "X",
        Content = request.Content,
        MediaUrl = request.MediaUrl,
        LinkUrl = request.LinkUrl
    };

    await provider.Post(post, ct);

    return Results.Ok(new { message = "Posted successfully" });
});

app.MapGet("/api/social/statistics/{tweetId}", async (
    string tweetId,
    ISocialProvider provider,
    CancellationToken ct) =>
{
    var stats = await provider.GetStatistics(tweetId, ct);
    return Results.Ok(stats);
});

record PostRequest(string Content, string? MediaUrl, string? LinkUrl);
```

## Blazor Component Example

```razor
@inject ISocialProvider XProvider
@inject ILogger<PostToX> Logger

<div class="post-form">
    <textarea @bind="content" placeholder="What's happening?" maxlength="280"></textarea>
    <button @onclick="PostAsync" disabled="@isPosting">
        @(isPosting ? "Posting..." : "Post to X")
    </button>
    
    @if (!string.IsNullOrEmpty(errorMessage))
    {
        <div class="error">@errorMessage</div>
    }
</div>

@code {
    private string content = "";
    private bool isPosting = false;
    private string? errorMessage = null;

    private async Task PostAsync()
    {
        isPosting = true;
        errorMessage = null;

        try
        {
            var post = new Post
            {
                Platform = "X",
                Content = content,
                MediaUrl = null,
                LinkUrl = null
            };

            await XProvider.Post(post, CancellationToken.None);
            
            content = "";
            Logger.LogInformation("Posted to X successfully");
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            Logger.LogError(ex, "Failed to post to X");
        }
        finally
        {
            isPosting = false;
        }
    }
}
```

## See Also

- [Configuration.md](Configuration.md) - Configuration options (flat vs nested)
- [Troubleshooting.md](Troubleshooting.md) - Common issues and solutions
