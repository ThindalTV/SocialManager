# BlueSky Provider Troubleshooting

## Common Issues and Solutions

### Authentication Errors

#### 401 Unauthorized / Authentication Failed

**Symptoms:**
- `SocialProviderAuthenticationException: Failed to authenticate with BlueSky`
- Cannot post or retrieve data
- "Invalid credentials" error

**Causes:**
- Invalid identifier (handle) format
- Incorrect or revoked app password
- Wrong PDS endpoint
- Handle doesn't match the app password

**Solutions:**

1. **Verify identifier format**:
   ```csharp
   // Correct formats:
   "alice.bsky.social"           // Handle
   "did:plc:abc123xyz456"        // DID
   
   // Incorrect:
   "@alice.bsky.social"          // Don't include @
   "alice"                        // Must be full handle
   ```

2. **Check app password**:
   - Ensure you copied the full password including dashes: `xxxx-xxxx-xxxx-xxxx`
   - Check for extra spaces at start or end
   - Verify the app password hasn't been revoked in BlueSky settings
   - Generate a new app password if uncertain

3. **Verify PDS URL**:
   ```json
   {
     "BlueSkyProvider": {
       "PdsUrl": "https://bsky.social"  // Default, most users
     }
   }
   ```

4. **Test authentication**:
   ```csharp
   try
   {
       var user = await _blueSkyClient.GetAuthenticatedUserAsync(ct);
       Console.WriteLine($"Success! Connected as: {user.Handle}");
   }
   catch (Exception ex)
   {
       Console.WriteLine($"Auth failed: {ex.Message}");
   }
   ```

#### Example Fix:
```bash
# Check your configuration
dotnet user-secrets list --project src/SocialManager/SocialProviders/SocialManager.SocialProvider.BlueSky

# Update if needed
dotnet user-secrets set "BlueSkyProvider:Identifier" "alice.bsky.social"
dotnet user-secrets set "BlueSkyProvider:AppPassword" "xxxx-xxxx-xxxx-xxxx"
```

### Rate Limit Errors

#### 429 Rate Limit Exceeded

**Symptoms:**
- `SocialProviderRateLimitException: Rate limit exceeded`
- Temporary inability to post or retrieve data
- "Too many requests" error

**Causes:**
- Too many requests in a short time period
- BlueSky enforcing protective rate limits

**Rate Limits (Approximate):**
| Operation | Estimated Limit |
|-----------|-----------------|
| Post | 50 posts per 15 minutes |
| Read/Get | 300-500 per 15 minutes |
| Media Upload | Subject to size/count limits |

?? **Note**: Rate limits are subject to change and may vary.

**Solutions:**

1. **Enable automatic retry** (default):
```json
{
  "BlueSkyProvider": {
    "EnableRetryOnRateLimit": true,
    "MaxRetryAttempts": 3
  }
}
```

2. **Implement exponential backoff**:
```csharp
int retryCount = 0;
const int maxRetries = 5;

while (retryCount < maxRetries)
{
    try
    {
        await provider.Post(post, ct);
        break;
    }
    catch (SocialProviderRateLimitException)
    {
        retryCount++;
        var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
        _logger.LogWarning("Rate limited, waiting {Delay}s", delay.TotalSeconds);
        await Task.Delay(delay, ct);
    }
}
```

3. **Implement caching** for read operations:
```csharp
private readonly IMemoryCache _cache;

public async Task<Statistic> GetStatisticsWithCacheAsync(string postUri)
{
    return await _cache.GetOrCreateAsync($"bluesky-stats-{postUri}", async entry =>
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
        return await _provider.GetStatistics(postUri, CancellationToken.None);
    });
}
```

4. **Queue posts** instead of rapid posting:
```csharp
public class PostQueue
{
    private readonly SemaphoreSlim _semaphore = new(1);
    
    public async Task QueuePostAsync(Post post)
    {
        await _semaphore.WaitAsync();
        try
        {
            await _provider.Post(post, CancellationToken.None);
            await Task.Delay(TimeSpan.FromSeconds(2)); // Throttle
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
```

### Content Errors

#### Post Too Long

**Symptoms:**
- `SocialProviderException: Post content exceeds maximum length of 300 characters`

**Causes:**
- Content + link exceeds 300 characters
- Not accounting for appended links

**Solutions:**

1. **Validate before posting**:
```csharp
private string TruncateContent(string content, int maxLength = 300)
{
    if (content.Length <= maxLength)
        return content;
    
    return content[..(maxLength - 3)] + "...";
}
```

2. **Check total length including links**:
```csharp
var totalLength = content.Length;
if (!string.IsNullOrEmpty(linkUrl))
{
    totalLength += 2 + linkUrl.Length; // \n\n + link
}

if (totalLength > 300)
{
    // Adjust content or remove link
}
```

3. **Use link shorteners** for long URLs:
```csharp
public async Task<string> ShortenUrlAsync(string longUrl)
{
    // Use a URL shortening service
    // e.g., bit.ly, TinyURL, etc.
}
```

#### Empty Content Error

**Symptoms:**
- `SocialProviderException: Post content cannot be empty`

**Cause:**
- Content is null, empty, or only whitespace

**Solution:**
```csharp
if (string.IsNullOrWhiteSpace(content))
{
    throw new ArgumentException("Content is required");
}
```

### Media Errors

#### Media Upload Failed

**Symptoms:**
- `SocialProviderException: Failed to upload media`
- Media not appearing in post
- Timeout during upload

**Causes:**
- Unsupported format
- File too large
- Invalid URL or file path
- Network issues

**Supported Formats:**
- Images: JPEG, PNG, GIF, WebP
- Max size: Approximately 1MB per image
- Recommended: < 5MB total

**Solutions:**

1. **Validate media before uploading**:
```csharp
private async Task<bool> ValidateImageAsync(string path)
{
    if (!File.Exists(path))
        return false;
    
    var info = new FileInfo(path);
    if (info.Length > 1 * 1024 * 1024) // 1MB
        return false;
    
    var extension = Path.GetExtension(path).ToLower();
    return extension is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp";
}
```

2. **Compress images** before upload:
```csharp
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public async Task<byte[]> CompressImageAsync(byte[] imageData)
{
    using var image = Image.Load(imageData);
    image.Mutate(x => x.Resize(new ResizeOptions
    {
        Size = new Size(1200, 0),
        Mode = ResizeMode.Max
    }));
    
    using var ms = new MemoryStream();
    await image.SaveAsJpegAsync(ms);
    return ms.ToArray();
}
```

3. **Handle download errors**:
```csharp
try
{
    using var httpClient = new HttpClient();
    httpClient.Timeout = TimeSpan.FromSeconds(30);
    var response = await httpClient.GetAsync(mediaUrl, ct);
    response.EnsureSuccessStatusCode();
    var mediaData = await response.Content.ReadAsByteArrayAsync(ct);
}
catch (HttpRequestException ex)
{
    _logger.LogError(ex, "Failed to download media from {Url}", mediaUrl);
    throw new SocialProviderException($"Could not download media: {ex.Message}", ex);
}
```

4. **Increase timeout** for large files:
```json
{
  "BlueSkyProvider": {
    "RequestTimeoutSeconds": 60
  }
}
```

### Configuration Errors

#### InvalidOperationException on Startup

**Symptoms:**
- `InvalidOperationException: BlueSkyProvider Identifier is required`
- Application fails to start
- Configuration not found

**Causes:**
- Missing required configuration
- Configuration not loaded
- Wrong section name

**Solutions:**

1. **Verify configuration is present**:
```bash
# Check appsettings.json
cat appsettings.json | grep BlueSkyProvider

# Or check user secrets
dotnet user-secrets list
```

2. **Ensure configuration is loaded**:
```csharp
// In Program.cs
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
```

3. **Validate section name matches**:
```csharp
// Must be "BlueSkyProvider", not "blueskyProvider" or "BlueSky"
builder.Services.AddBlueSkyProvider(builder.Configuration);

// With parent section
builder.Services.AddBlueSkyProvider(builder.Configuration, "SocialProviders");
```

#### Configuration Not Loading

**Symptoms:**
- All required fields set but still getting validation errors
- Default values being used instead of configured values

**Solutions:**

1. **Check configuration binding**:
```csharp
var config = builder.Configuration
    .GetSection("BlueSkyProvider")
    .Get<BlueSkyProviderConfiguration>();

if (config == null || string.IsNullOrEmpty(config.Identifier))
{
    Console.WriteLine("Configuration not loaded!");
}
```

2. **Verify environment-specific config**:
```bash
# Set environment
export ASPNETCORE_ENVIRONMENT=Development

# Check which appsettings file is being used
```

### Connection Errors

#### Unable to Connect to BlueSky API

**Symptoms:**
- Timeout errors
- Network-related exceptions
- "Connection refused" errors

**Solutions:**

1. **Check network connectivity**:
```bash
# Ping BlueSky PDS
ping bsky.social

# Check HTTPS access
curl https://bsky.social
```

2. **Verify firewall/proxy settings**:
   - Ensure outbound HTTPS (port 443) is allowed
   - Check corporate firewall rules
   - Verify proxy configuration if applicable

3. **Check BlueSky service status**:
   - Visit https://bsky.app in a browser
   - Check BlueSky's status page or social media

4. **Increase timeout** if on slow connection:
```json
{
  "BlueSkyProvider": {
    "RequestTimeoutSeconds": 60
  }
}
```

### AT URI Issues

#### Invalid AT URI Format

**Symptoms:**
- Cannot retrieve post statistics
- "Invalid URI" errors

**Correct AT URI Format:**
```
at://did:plc:abc123xyz/app.bsky.feed.post/3klmnopqrst
```

**Common Mistakes:**
```
// Wrong - missing protocol
did:plc:abc123xyz/app.bsky.feed.post/3klmnopqrst

// Wrong - HTTP URL instead of AT URI
https://bsky.app/profile/alice.bsky.social/post/3klmnopqrst

// Wrong - malformed DID
at://alice.bsky.social/app.bsky.feed.post/3klmnopqrst
```

**Solution:**
```csharp
// When posting, save the returned URI
var response = await _client.PublishPostAsync(content, ct);
var atUri = response.Uri; // Correct format

// Later use this URI to get statistics
var stats = await provider.GetStatistics(atUri, ct);
```

## Debugging Tips

### Enable Detailed Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "SocialManager.SocialProvider.BlueSky": "Debug",
      "FishyFlip": "Debug"
    }
  }
}
```

### Test Authentication Endpoint

Create a simple test endpoint:

```csharp
app.MapGet("/test/bluesky-auth", async (IBlueSkyApiClient client) =>
{
    try
    {
        var user = await client.GetAuthenticatedUserAsync(CancellationToken.None);
        return Results.Ok(new 
        { 
            success = true, 
            handle = user.Handle,
            did = user.Did,
            displayName = user.DisplayName,
            followers = user.FollowersCount
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new 
        { 
            success = false, 
            error = ex.Message,
            type = ex.GetType().Name
        });
    }
});
```

### Inspect Configuration at Runtime

```csharp
app.MapGet("/debug/bluesky-config", (IOptions<BlueSkyProviderConfiguration> config) =>
{
    var c = config.Value;
    return Results.Ok(new
    {
        active = c.Active,
        platform = c.Platform,
        hasIdentifier = !string.IsNullOrEmpty(c.Identifier),
        identifierValue = c.Identifier, // Remove in production!
        hasAppPassword = !string.IsNullOrEmpty(c.AppPassword),
        pdsUrl = c.PdsUrl,
        enableRetry = c.EnableRetryOnRateLimit,
        maxRetries = c.MaxRetryAttempts,
        timeoutSeconds = c.RequestTimeoutSeconds
    });
});
```

?? **Security Warning**: Remove debug endpoints in production!

### Test Post Endpoint

```csharp
app.MapPost("/test/bluesky-post", async (
    ISocialProvider provider,
    [FromBody] string content) =>
{
    try
    {
        var post = new Post
        {
            Platform = "BlueSky",
            Content = content
        };
        
        await provider.Post(post, CancellationToken.None);
        return Results.Ok(new { success = true });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new 
        { 
            success = false,
            error = ex.Message,
            stackTrace = ex.StackTrace
        });
    }
});
```

## Getting Help

### Resources
- [BlueSky Official Site](https://bsky.app)
- [AT Protocol Documentation](https://atproto.com)
- [AT Protocol Specs](https://atproto.com/specs/atp)
- [FishyFlip GitHub](https://github.com/drasticactions/FishyFlip)
- [BlueSky Developer Discord](https://discord.gg/bluesky)

### Reporting Issues

When reporting issues, include:
1. Error message and stack trace
2. BlueSky Provider version and FishyFlip version
3. Configuration (without secrets!)
4. Steps to reproduce
5. Relevant log output

### Example Issue Report

```
**Environment:**
- .NET Version: 10.0
- BlueSky Provider: 1.0.0
- FishyFlip: 4.2.0-alpha.2
- OS: Windows 11

**Configuration:**
- PDS URL: https://bsky.social
- Nested config: Yes
- Parent section: SocialProviders

**Error:**
SocialProviderAuthenticationException: Failed to authenticate with BlueSky

**Steps to Reproduce:**
1. Configure identifier and app password
2. Call GetAuthenticatedUserAsync
3. Exception thrown

**Logs:**
[Include relevant log output]
```

## See Also
- [API-Setup.md](API-Setup.md) - Getting credentials
- [Configuration.md](Configuration.md) - Configuration options
- [Usage-Examples.md](Usage-Examples.md) - Code examples
