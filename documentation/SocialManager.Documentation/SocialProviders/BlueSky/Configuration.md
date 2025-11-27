# BlueSky Provider Configuration

## Overview
This guide explains how to configure the BlueSky provider in your SocialManager application.

## Configuration Structure

The BlueSky Provider supports both **flat** and **nested** configuration structures.

### Flat Structure (Default)

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

```csharp
// Registration
builder.Services.AddBlueSkyProvider(builder.Configuration);
```

### Nested Structure (Recommended for Multiple Providers)

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

```csharp
// Registration with parent section
builder.Services.AddBlueSkyProvider(builder.Configuration, "SocialProviders");
```

## Configuration Properties

| Property | Type | Required | Default | Description |
|----------|------|----------|---------|-------------|
| `Active` | bool | No | `true` | Enable/disable the provider |
| `Platform` | string | No | `"BlueSky"` | Platform identifier |
| `Identifier` | string | Yes | - | BlueSky handle (e.g., `alice.bsky.social`) or DID |
| `AppPassword` | string | Yes | - | App password from BlueSky settings |
| `PdsUrl` | string | No | `"https://bsky.social"` | Personal Data Server endpoint |
| `EnableRetryOnRateLimit` | bool | No | `true` | Automatically retry on rate limit |
| `MaxRetryAttempts` | int | No | `3` | Maximum retry attempts |
| `RequestTimeoutSeconds` | int | No | `30` | API request timeout in seconds |

## Secure Configuration

### Development: User Secrets

Use .NET User Secrets for development:

```bash
dotnet user-secrets init --project src/SocialManager/SocialProviders/SocialManager.SocialProvider.BlueSky

# For flat structure
dotnet user-secrets set "BlueSkyProvider:Identifier" "alice.bsky.social"
dotnet user-secrets set "BlueSkyProvider:AppPassword" "xxxx-xxxx-xxxx-xxxx"
dotnet user-secrets set "BlueSkyProvider:PdsUrl" "https://bsky.social"

# For nested structure
dotnet user-secrets set "SocialProviders:BlueSkyProvider:Identifier" "alice.bsky.social"
dotnet user-secrets set "SocialProviders:BlueSkyProvider:AppPassword" "xxxx-xxxx-xxxx-xxxx"
dotnet user-secrets set "SocialProviders:BlueSkyProvider:PdsUrl" "https://bsky.social"
```

### Production: Environment Variables

Set environment variables:

**Flat Structure - Windows (PowerShell)**:
```powershell
$env:BlueSkyProvider__Identifier="alice.bsky.social"
$env:BlueSkyProvider__AppPassword="xxxx-xxxx-xxxx-xxxx"
$env:BlueSkyProvider__PdsUrl="https://bsky.social"
```

**Nested Structure - Windows (PowerShell)**:
```powershell
$env:SocialProviders__BlueSkyProvider__Identifier="alice.bsky.social"
$env:SocialProviders__BlueSkyProvider__AppPassword="xxxx-xxxx-xxxx-xxxx"
$env:SocialProviders__BlueSkyProvider__PdsUrl="https://bsky.social"
```

**Flat Structure - Linux/macOS (Bash)**:
```bash
export BlueSkyProvider__Identifier="alice.bsky.social"
export BlueSkyProvider__AppPassword="xxxx-xxxx-xxxx-xxxx"
export BlueSkyProvider__PdsUrl="https://bsky.social"
```

**Nested Structure - Linux/macOS (Bash)**:
```bash
export SocialProviders__BlueSkyProvider__Identifier="alice.bsky.social"
export SocialProviders__BlueSkyProvider__AppPassword="xxxx-xxxx-xxxx-xxxx"
export SocialProviders__BlueSkyProvider__PdsUrl="https://bsky.social"
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
- `BlueSkyProvider--Identifier`
- `BlueSkyProvider--AppPassword`
- `BlueSkyProvider--PdsUrl`

**Nested Structure - Secret names**:
- `SocialProviders--BlueSkyProvider--Identifier`
- `SocialProviders--BlueSkyProvider--AppPassword`
- `SocialProviders--BlueSkyProvider--PdsUrl`

## Service Registration

### Flat Structure (Default)

```csharp
using SocialManager.SocialProvider.BlueSky.Extensions;

// Register with flat configuration structure
builder.Services.AddBlueSkyProvider(builder.Configuration);
```

### Nested Structure

```csharp
using SocialManager.SocialProvider.BlueSky.Extensions;

// Register with nested configuration structure
builder.Services.AddBlueSkyProvider(builder.Configuration, "SocialProviders");

// Or with a different parent section
builder.Services.AddBlueSkyProvider(builder.Configuration, "ExternalServices");
```

### Programmatic Configuration

Configure directly in code (not recommended for production):

```csharp
builder.Services.AddBlueSkyProvider(options =>
{
    options.Active = true;
    options.Platform = "BlueSky";
    options.Identifier = "alice.bsky.social";
    options.AppPassword = "xxxx-xxxx-xxxx-xxxx";
    options.PdsUrl = "https://bsky.social";
    options.EnableRetryOnRateLimit = true;
    options.MaxRetryAttempts = 3;
    options.RequestTimeoutSeconds = 30;
});
```

## Advanced Configuration

### Custom PDS Instance

If you're using a self-hosted or custom PDS:

```json
{
  "BlueSkyProvider": {
    "Identifier": "alice.mydomain.com",
    "AppPassword": "xxxx-xxxx-xxxx-xxxx",
    "PdsUrl": "https://pds.mydomain.com"
  }
}
```

### Disable Automatic Retry

For applications with custom retry logic:

```json
{
  "BlueSkyProvider": {
    "Identifier": "alice.bsky.social",
    "AppPassword": "xxxx-xxxx-xxxx-xxxx",
    "EnableRetryOnRateLimit": false,
    "MaxRetryAttempts": 0
  }
}
```

### Extended Timeout

For slower networks or large media uploads:

```json
{
  "BlueSkyProvider": {
    "Identifier": "alice.bsky.social",
    "AppPassword": "xxxx-xxxx-xxxx-xxxx",
    "RequestTimeoutSeconds": 60
  }
}
```

## Security Best Practices

1. **Never commit credentials** to source control
   - Add `appsettings.Production.json` to `.gitignore`
   - Use secure storage for all environments

2. **Rotate app passwords regularly**
   - Generate new passwords every 90 days
   - Immediately rotate if compromised

3. **Use separate passwords per environment**
   - Different passwords for dev, staging, production
   - Makes it easier to manage and revoke

4. **Monitor API usage**
   - Set up alerts for unusual activity
   - Track API call patterns and rate limits

5. **Principle of least privilege**
   - Use app passwords (limited scope) instead of main password
   - Revoke unused app passwords

## Validation

The configuration is automatically validated when the BlueSkyApiClient is created. Invalid configurations will throw an `InvalidOperationException` with a descriptive message.

### Common Validation Errors

**Missing Identifier**:
```
InvalidOperationException: BlueSkyProvider Identifier is required.
```

**Missing App Password**:
```
InvalidOperationException: BlueSkyProvider AppPassword is required.
```

**Invalid Max Retry Attempts**:
```
InvalidOperationException: BlueSkyProvider MaxRetryAttempts must be non-negative.
```

**Invalid Request Timeout**:
```
InvalidOperationException: BlueSkyProvider RequestTimeoutSeconds must be positive.
```

## Multiple Provider Example

When using multiple social providers, the nested structure is recommended:

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
    },
    "MastodonProvider": {
      "Active": false,
      "InstanceUrl": "https://mastodon.social",
      "AccessToken": "mastodon-token"
    }
  }
}
```

```csharp
// Register all providers with the same parent section
builder.Services.AddBlueSkyProvider(builder.Configuration, "SocialProviders");
builder.Services.AddXProvider(builder.Configuration, "SocialProviders");
builder.Services.AddMastodonProvider(builder.Configuration, "SocialProviders");
```

## Configuration per Environment

### appsettings.Development.json
```json
{
  "BlueSkyProvider": {
    "Identifier": "testaccount.bsky.social",
    "AppPassword": "dev-xxxx-xxxx-xxxx-xxxx",
    "EnableRetryOnRateLimit": false
  }
}
```

### appsettings.Production.json
```json
{
  "BlueSkyProvider": {
    "Active": true,
    "EnableRetryOnRateLimit": true,
    "MaxRetryAttempts": 5,
    "RequestTimeoutSeconds": 45
  }
}
```

?? **Note**: Never commit sensitive values. Use environment variables or Key Vault in production.

## Next Steps

See [Usage-Examples.md](Usage-Examples.md) for code examples.

## See Also

- [API-Setup.md](API-Setup.md) - Getting BlueSky credentials
- [Usage-Examples.md](Usage-Examples.md) - Code examples
- [Troubleshooting.md](Troubleshooting.md) - Common issues and solutions
