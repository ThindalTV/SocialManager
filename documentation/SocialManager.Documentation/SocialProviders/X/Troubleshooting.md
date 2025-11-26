# X Provider Troubleshooting

## Common Issues and Solutions

### Authentication Errors

#### 401 Unauthorized

**Symptoms:**
- `XAuthenticationException: Authentication failed`
- Cannot post or retrieve tweets

**Causes:**
- Invalid API credentials
- Tokens regenerated but old tokens still in use
- Incorrect token permissions

**Solutions:**
1. Verify all four credentials are correct:
   - API Key (Consumer Key)
   - API Secret (Consumer Secret)
   - Access Token
   - Access Token Secret

2. Check for extra spaces or quotes in configuration

3. If app permissions changed, regenerate Access Token and Secret:
   - Go to X Developer Portal ? Your App ? Keys and tokens
   - Regenerate Access Token and Secret
   - Update your configuration

4. Verify app has "Read and write" permissions in settings

#### Example Fix:
```bash
# Check your configuration
dotnet user-secrets list --project src/SocialManager/SocialProviders/SocialManager.SocialProvider.X

# Update if needed
dotnet user-secrets set "XProvider:AccessToken" "new-token"
dotnet user-secrets set "XProvider:AccessTokenSecret" "new-secret"
```

### Rate Limit Errors

#### 429 Rate Limit Exceeded

**Symptoms:**
- `XRateLimitException: Rate limit exceeded`
- Temporary inability to post or retrieve data

**Causes:**
- Too many requests in 15-minute window
- Free tier limits reached

**Rate Limits (Free Tier):**
| Operation | Per 15 min | Per month |
|-----------|------------|-----------|
| Post Tweet | 50 | 1,500 |
| Get Tweet | 900 | Unlimited |

**Solutions:**

1. **Enable automatic retry** (default):
```json
{
  "XProvider": {
    "EnableRetryOnRateLimit": true,
    "MaxRetryAttempts": 3
  }
}
```

2. **Implement caching** for read operations:
```csharp
private readonly IMemoryCache _cache;

public async Task<Statistic> GetStatisticsWithCacheAsync(string tweetId)
{
    return await _cache.GetOrCreateAsync($"tweet-stats-{tweetId}", async entry =>
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
        return await _provider.GetStatistics(tweetId, CancellationToken.None);
    });
}
```

3. **Handle reset time**:
```csharp
catch (XRateLimitException ex)
{
    if (ex.ResetTime.HasValue)
    {
        var waitTime = ex.ResetTime.Value - DateTime.UtcNow;
        _logger.LogWarning("Rate limit hit. Waiting {Minutes} minutes", waitTime.TotalMinutes);
        await Task.Delay(waitTime);
    }
}
```

4. **Upgrade tier** if consistently hitting limits:
   - Basic: $100/month
   - Pro: $5,000/month

### Content Errors

#### Tweet Too Long

**Symptoms:**
- `XProviderException: Tweet content exceeds maximum length of 280 characters`

**Causes:**
- Content + link exceeds 280 characters
- Not accounting for URL shortening

**Solutions:**

1. **Validate before posting**:
```csharp
private string TruncateContent(string content, int maxLength = 280)
{
    if (content.Length <= maxLength)
        return content;
    
    return content[..(maxLength - 3)] + "...";
}
```

2. **Remember**: URLs count as 23 characters regardless of actual length

3. **Check total length including links**:
```csharp
var estimatedLength = content.Length + (linkUrl != null ? 23 : 0);
if (estimatedLength > 280)
{
    // Adjust content
}
```

#### Duplicate Tweet

**Symptoms:**
- 403 Forbidden error
- "Status is a duplicate" message

**Cause:**
- X prevents posting identical content in a short time

**Solution:**
- Wait a few minutes before posting identical content
- Add unique elements (timestamp, emoji, etc.)

### Media Errors

#### Media Upload Failed

**Symptoms:**
- `XProviderException: Failed to upload media`
- Media not appearing in tweet

**Causes:**
- Unsupported format
- File too large
- Invalid URL or file path

**Supported Formats:**
- Images: JPEG, PNG, GIF, WEBP
- Max size: 5MB (images), 512MB (videos)

**Solutions:**

1. **Validate media before uploading**:
```csharp
private async Task<bool> ValidateImageAsync(string path)
{
    if (!File.Exists(path))
        return false;
    
    var info = new FileInfo(path);
    if (info.Length > 5 * 1024 * 1024) // 5MB
        return false;
    
    var extension = Path.GetExtension(path).ToLower();
    return extension is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp";
}
```

2. **Handle download errors**:
```csharp
try
{
    using var httpClient = new HttpClient();
    httpClient.Timeout = TimeSpan.FromSeconds(30);
    var mediaData = await httpClient.GetByteArrayAsync(mediaUrl);
}
catch (HttpRequestException ex)
{
    _logger.LogError(ex, "Failed to download media from {Url}", mediaUrl);
    throw new XProviderException($"Could not download media: {ex.Message}", ex);
}
```

### Configuration Errors

#### InvalidOperationException on Startup

**Symptoms:**
- `InvalidOperationException: XProvider ApiKey is required`
- Application fails to start

**Causes:**
- Missing required configuration
- Configuration not loaded

**Solutions:**

1. **Verify configuration is present**:
```bash
# Check appsettings.json or user secrets
cat appsettings.json | grep XProvider
```

2. **Ensure configuration is loaded**:
```csharp
// In Program.cs
builder.Configuration.AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
```

3. **Validate configuration section name matches**:
```csharp
// Must be "XProvider", not "xProvider" or "X"
builder.Services.Configure<XProviderConfiguration>(
    builder.Configuration.GetSection("XProvider"));
```

### Connection Errors

#### Unable to Connect to X API

**Symptoms:**
- Timeout errors
- Network-related exceptions

**Solutions:**

1. **Check network connectivity**:
```bash
ping api.twitter.com
```

2. **Verify firewall/proxy settings** allow HTTPS to api.twitter.com

3. **Check X API status**: https://api.twitterstat.us/

4. **Increase timeout** if on slow connection:
```csharp
// The TwitterClient uses default HttpClient timeout (100 seconds)
// If needed, this is handled internally by Tweetinvi
```

## Debugging Tips

### Enable Detailed Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "SocialManager.SocialProvider.X": "Debug"
    }
  }
}
```

### Test Authentication

Create a simple test endpoint:

```csharp
app.MapGet("/test/x-auth", async (IXApiClient client) =>
{
    try
    {
        var user = await client.GetAuthenticatedUserAsync(CancellationToken.None);
        return Results.Ok(new 
        { 
            success = true, 
            username = user.ScreenName,
            userId = user.Id 
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new 
        { 
            success = false, 
            error = ex.Message 
        });
    }
});
```

### Inspect Configuration at Runtime

```csharp
app.MapGet("/debug/x-config", (IOptions<XProviderConfiguration> config) =>
{
    var c = config.Value;
    return Results.Ok(new
    {
        active = c.Active,
        platform = c.Platform,
        hasApiKey = !string.IsNullOrEmpty(c.ApiKey),
        hasApiSecret = !string.IsNullOrEmpty(c.ApiSecret),
        hasAccessToken = !string.IsNullOrEmpty(c.AccessToken),
        hasAccessTokenSecret = !string.IsNullOrEmpty(c.AccessTokenSecret),
        enableRetry = c.EnableRetryOnRateLimit,
        maxRetries = c.MaxRetryAttempts
    });
});
```

?? **Security Warning**: Remove debug endpoints in production!

## Getting Help

### Resources
- [X API Documentation](https://developer.twitter.com/en/docs)
- [X Developer Community](https://twittercommunity.com/)
- [Tweetinvi GitHub Issues](https://github.com/linvi/tweetinvi/issues)

### Reporting Issues
When reporting issues, include:
1. Error message and stack trace
2. X Provider version
3. Configuration (without secrets!)
4. Steps to reproduce

## See Also
- [API-Setup.md](API-Setup.md) - Getting credentials
- [Configuration.md](Configuration.md) - Configuration options
- [Usage-Examples.md](Usage-Examples.md) - Code examples
