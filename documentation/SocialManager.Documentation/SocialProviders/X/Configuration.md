# X Provider Configuration

## Overview
This guide explains how to configure the X (Twitter) provider in your SocialManager application.

## Configuration Structure

The X Provider supports both **flat** and **nested** configuration structures.

### Flat Structure (Default)

```json
{
  "XProvider": {
    "Active": true,
    "Platform": "X",
    "ApiKey": "your-api-key-here",
    "ApiSecret": "your-api-secret-here",
    "AccessToken": "your-access-token-here",
    "AccessTokenSecret": "your-access-token-secret-here"
  }
}
```

```csharp
// Registration
builder.Services.AddXProvider(builder.Configuration);
```

### Nested Structure (Recommended for Multiple Providers)

```json
{
  "SocialProviders": {
    "XProvider": {
      "Active": true,
      "Platform": "X",
      "ApiKey": "your-api-key-here",
      "ApiSecret": "your-api-secret-here",
      "AccessToken": "your-access-token-here",
      "AccessTokenSecret": "your-access-token-secret-here"
    }
  }
}
```

```csharp
// Registration with parent section
builder.Services.AddXProvider(builder.Configuration, "SocialProviders");
```

## Configuration Properties

| Property | Type | Required | Default | Description |
|----------|------|----------|---------|-------------|
| `Active` | bool | No | `true` | Enable/disable the provider |
| `Platform` | string | No | `"X"` | Platform identifier |
| `ApiKey` | string | Yes | - | API Key (Consumer Key) from X Developer Portal |
| `ApiSecret` | string | Yes | - | API Secret (Consumer Secret) |
| `AccessToken` | string | Yes | - | Access Token for authenticated user |
| `AccessTokenSecret` | string | Yes | - | Access Token Secret |
| `BearerToken` | string | No | `null` | Optional Bearer Token for OAuth 2.0 |
| `EnableRetryOnRateLimit` | bool | No | `true` | Automatically retry on rate limit |
| `MaxRetryAttempts` | int | No | `3` | Maximum retry attempts |

## Secure Configuration

### Development: User Secrets

Use .NET User Secrets for development:

```bash
dotnet user-secrets init --project src/SocialManager/SocialProviders/SocialManager.SocialProvider.X

# For flat structure
dotnet user-secrets set "XProvider:ApiKey" "your-api-key"
dotnet user-secrets set "XProvider:ApiSecret" "your-api-secret"
dotnet user-secrets set "XProvider:AccessToken" "your-access-token"
dotnet user-secrets set "XProvider:AccessTokenSecret" "your-access-token-secret"

# For nested structure
dotnet user-secrets set "SocialProviders:XProvider:ApiKey" "your-api-key"
dotnet user-secrets set "SocialProviders:XProvider:ApiSecret" "your-api-secret"
dotnet user-secrets set "SocialProviders:XProvider:AccessToken" "your-access-token"
dotnet user-secrets set "SocialProviders:XProvider:AccessTokenSecret" "your-access-token-secret"
```

### Production: Environment Variables

Set environment variables:

**Flat Structure - Windows (PowerShell)**:
```powershell
$env:XProvider__ApiKey="your-api-key"
$env:XProvider__ApiSecret="your-api-secret"
$env:XProvider__AccessToken="your-access-token"
$env:XProvider__AccessTokenSecret="your-access-token-secret"
```

**Nested Structure - Windows (PowerShell)**:
```powershell
$env:SocialProviders__XProvider__ApiKey="your-api-key"
$env:SocialProviders__XProvider__ApiSecret="your-api-secret"
$env:SocialProviders__XProvider__AccessToken="your-access-token"
$env:SocialProviders__XProvider__AccessTokenSecret="your-access-token-secret"
```

**Flat Structure - Linux/macOS (Bash)**:
```bash
export XProvider__ApiKey="your-api-key"
export XProvider__ApiSecret="your-api-secret"
export XProvider__AccessToken="your-access-token"
export XProvider__AccessTokenSecret="your-access-token-secret"
```

**Nested Structure - Linux/macOS (Bash)**:
```bash
export SocialProviders__XProvider__ApiKey="your-api-key"
export SocialProviders__XProvider__ApiSecret="your-api-secret"
export SocialProviders__XProvider__AccessToken="your-access-token"
export SocialProviders__XProvider__AccessTokenSecret="your-access-token-secret"
```

### Production: Azure Key Vault

Store credentials in Azure Key Vault:

```csharp
// In Program.cs
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```

**Flat Structure - Secret names**:
- `XProvider--ApiKey`
- `XProvider--ApiSecret`
- `XProvider--AccessToken`
- `XProvider--AccessTokenSecret`

**Nested Structure - Secret names**:
- `SocialProviders--XProvider--ApiKey`
- `SocialProviders--XProvider--ApiSecret`
- `SocialProviders--XProvider--AccessToken`
- `SocialProviders--XProvider--AccessTokenSecret`

## Service Registration

### Flat Structure (Default)

```csharp
using SocialManager.SocialProvider.X.Extensions;

// Register with flat configuration structure
builder.Services.AddXProvider(builder.Configuration);
```

### Nested Structure

```csharp
using SocialManager.SocialProvider.X.Extensions;

// Register with nested configuration structure
builder.Services.AddXProvider(builder.Configuration, "SocialProviders");

// Or with a different parent section
builder.Services.AddXProvider(builder.Configuration, "ExternalServices");
```

### Programmatic Configuration

Configure directly in code (not recommended for production):

```csharp
builder.Services.AddXProvider(options =>
{
    options.Active = true;
    options.Platform = "X";
    options.ApiKey = "your-api-key";
    options.ApiSecret = "your-api-secret";
    options.AccessToken = "your-access-token";
    options.AccessTokenSecret = "your-access-token-secret";
    options.EnableRetryOnRateLimit = true;
    options.MaxRetryAttempts = 3;
});
```

## Security Best Practices

1. **Never commit credentials** to source control
   - Add `appsettings.Production.json` to `.gitignore`
   - Use secure storage for all environments

2. **Rotate credentials regularly**
   - Regenerate tokens every 90 days
   - Immediately rotate if compromised

3. **Use minimum required permissions**
   - Only enable "Read and write" if posting is needed
   - Consider separate tokens for different environments

4. **Monitor API usage**
   - Set up alerts for unusual activity
   - Track API call patterns

## Validation

The configuration is automatically validated when the XApiClient is created. Invalid configurations will throw an `InvalidOperationException` with a descriptive message.

## Multiple Provider Example

When using multiple social providers, the nested structure is recommended:

```json
{
  "SocialProviders": {
    "XProvider": {
      "Active": true,
      "ApiKey": "x-api-key"
    },
    "FacebookProvider": {
      "Active": true,
      "AppId": "facebook-app-id"
    },
    "LinkedInProvider": {
      "Active": false,
      "ClientId": "linkedin-client-id"
    }
  }
}
```

```csharp
// Register all providers with the same parent section
builder.Services.AddXProvider(builder.Configuration, "SocialProviders");
builder.Services.AddFacebookProvider(builder.Configuration, "SocialProviders");
builder.Services.AddLinkedInProvider(builder.Configuration, "SocialProviders");
```

## Next Steps

See [Usage-Examples.md](Usage-Examples.md) for code examples.
