# User Management

## Overview
SocialManager is designed as a **multi-user, multi-tenant** platform where each user maintains their own isolated workspace with custom settings, content, and social media connections. The system supports multiple users managing their own social media accounts, blog posts, and streaming configurations independently.

## Architecture Principles

### Multi-Tenancy Model
- Social manager can be multi tenant, but it is always multi user. If installed locally, it will default to single tenant mode, but it can also be run as a public service with multitenant functionality.
- Each user has separate permissions, using the format area:permissionLevel. For example, blog:read, stream:moderate etc...

### Authentication Abstraction
The authentication system is built on an **abstraction layer** that allows the underlying authentication provider to be replaced without affecting the application logic:
- Core application depends on authentication **interfaces**, not implementations
- Current implementation uses **Auth0** but can be swapped for:
  - Azure AD B2C
  - AWS Cognito
  - IdentityServer
  - Custom OAuth/OIDC provider
  - Self-hosted identity solution

### Provider Independence
The authentication layer is implemented through three core abstractions:
- **IAuthenticationService** - Handles login, logout, token validation, and refresh
- **IUserIdentityProvider** - Manages user identity and profile information
- **IAuthorizationService** - Handles authorization decisions and permission checks

Authentication providers are registered based on configuration, allowing runtime selection:
```json
{
  "Authentication": {
    "Provider": "Auth0",  // or "AzureAD", "Custom", etc.
    "Auth0": { /* provider-specific config */ }
  }
}
```

## User Data Model

### User Entity
Each user has a comprehensive profile including:

**Identity Information**
- Unique user ID (from authentication provider)
- Email address (required, unique)
- Display name and avatar URL
- Authentication provider reference

**Profile Details**
- Bio, website, location
- Timezone preference
- Usage statistics (login count, last login)

**Preferences & Settings**
- UI preferences (theme, language, date/time formats)
- Notification settings (email, platform-specific alerts)
- Content preferences (default visibility, auto-save, spell check)
- Privacy settings (profile visibility, analytics, data sharing)

**Relationships**
- Connected social media accounts
- Team member associations (future)
- Subscription tier and expiration

### Connected Social Media Accounts
Users can link multiple accounts per platform:

**Account Information**
- Platform identifier (X, Instagram, LinkedIn, etc.)
- Platform-specific user ID and username
- Display name and profile image
- Profile URL

**Authentication Tokens**
- Access token (encrypted at rest)
- Refresh token (if applicable)
- Token expiration tracking
- OAuth scopes/permissions

**Account Status**
- Active/inactive flag
- Verification status
- Last sync timestamp
- Error tracking for connection issues

**Per-Account Settings**
- Auto-publish toggle
- Analytics enablement
- Engagement notifications
- Platform-specific custom settings

## Multi-Tenant Data Isolation

### User Context Service
A user context service provides the authenticated user's identity throughout the application:
- Extracts user ID from authentication claims
- Provides email, display name, roles, and permissions
- Throws exception if accessed while unauthenticated
- Accessible via dependency injection in all services

### Automatic Data Filtering
All data repositories automatically scope queries to the current user:

**Repository Pattern Enhancement**
- Standard repository methods (`GetById`, `Get`, `Add`, `Update`, `Delete`) automatically filter by user
- User-scoped repositories enforce data isolation at the data access layer
- No additional filtering logic needed in application code
- Prevents accidental cross-user data access

**User-Owned Entity Contract**
All user-owned data entities implement a standard interface:
```csharp
public interface IUserOwnedEntity
{
    string UserId { get; }
}
```

This enables:
- Generic repository implementations
- Compile-time enforcement of user scoping
- Consistent data isolation patterns

### Data Isolation Examples
- **Blog Posts**: User can only read/write their own posts
- **Social Posts**: Scoped to user's connected accounts
- **Media Library**: User's uploaded images and videos
- **Scheduled Content**: User's publishing schedule
- **Analytics Data**: User's performance metrics
- **Settings & Preferences**: User-specific configuration

## Authorization & Permissions

### Role-Based Access Control (RBAC)
SocialManager implements a hierarchical role system with increasing privilege levels:

1. **Viewer** - Read-only access to content
2. **Editor** - Create and edit content (cannot publish)
3. **Publisher** - Full content creation and publishing
4. **Admin** - Workspace administration (settings, team management)
5. **Owner** - Account owner with billing and security access

### Permission Model
Fine-grained permissions supplement role-based access:

**Blog Permissions**
- `blog:read`, `blog:write`, `blog:publish`, `blog:delete`

**Social Media Permissions**
- `social:read`, `social:write`, `social:publish`, `social:delete`

**Account Management Permissions**
- `account:manage`, `account:connect`, `account:disconnect`

**Settings Permissions**
- `settings:read`, `settings:write`

**Team Permissions** (future)
- `team:manage`, `team:invite`

### Authorization Policies
Common authorization scenarios are defined as reusable policies:
- **CanPublishContent** - Required for publishing to blog or social platforms
- **CanManageAccount** - Required for connecting/disconnecting accounts
- **CanInviteTeamMembers** - Required for team management
- **IsAccountOwner** - Owner-only operations (billing, account deletion)

### Controller-Level Authorization
API endpoints use standard ASP.NET Core authorization:
- `[Authorize]` attribute requires authentication
- `[Authorize(Policy = "...")]` enforces specific policies
- Manual ownership checks verify user owns requested resources
- Returns `403 Forbidden` for unauthorized access attempts

## User Settings Management

### Settings Architecture
User preferences are stored in a hierarchical structure:

**UI Preferences**
- Theme (dark, light, auto)
- Language and localization
- Date and time formatting

**Notification Preferences**
- Email notification toggle
- Per-event notifications (published, scheduled, failed, weekly summary)
- Platform-specific notification settings

**Content Preferences**
- Default visibility (public, private, unlisted)
- Auto-save settings
- Spell check enablement
- Default hashtags
- Per-platform defaults (auto-post, formatting)

**Privacy Preferences**
- Profile visibility
- Activity visibility
- Analytics consent
- Data sharing with platforms

### Settings Service
A dedicated service manages user preferences:
- Get/update full preference object
- Get/set individual settings by key
- Reset to system defaults
- Type-safe setting retrieval with defaults
- Settings stored as JSON for flexibility

### Settings API
RESTful endpoints provide settings access:
- `GET /api/users/settings/preferences` - Retrieve all preferences
- `PUT /api/users/settings/preferences` - Update preferences
- `GET /api/users/settings/preferences/{key}` - Get specific setting
- `PUT /api/users/settings/preferences/{key}` - Set specific setting
- `POST /api/users/settings/preferences/reset` - Reset to defaults

## Blazor WebAssembly Authentication

### Authentication State Management
Blazor WebAssembly uses a custom authentication state provider:

**Responsibilities**
- Store JWT tokens in browser local storage
- Validate tokens before use
- Parse claims from JWT for user identity
- Add authorization header to HTTP requests
- Notify application of authentication state changes

**Token Lifecycle**
1. User logs in ? token stored in local storage
2. App starts ? token validated and claims extracted
3. API calls ? authorization header automatically added
4. Token expires ? user logged out automatically
5. User logs out ? token removed from storage

### Login Flow
The login component handles the authentication workflow:
1. User enters credentials
2. Credentials sent to authentication API
3. On success, access token returned
4. Token stored and user marked as authenticated
5. Application redirects to home page
6. Authorization header set for all subsequent requests

### Security Considerations
- Tokens stored in browser local storage (XSS risk mitigation required)
- Token validation on every app startup
- Automatic logout on token expiration
- HTTPS required for all authentication operations
- Consider refresh token rotation for long-lived sessions

## Team Collaboration (Future)

### Team Member Support
Future versions will support multiple users collaborating on a single account:

**Team Member Model**
- Account owner can invite team members by email
- Each member has a role (Viewer, Editor, Publisher, Admin)
- Custom permission sets override role defaults
- Invitation workflow with email tokens
- Active, invited, suspended, and removed states

**Use Cases**
- Social media managers with multiple team members
- Agencies managing client accounts
- Content teams with editors and publishers
- Marketing departments with approval workflows

## Security Best Practices

### Token Management
- **Access Tokens**: Short-lived (1 hour) JWT tokens for API authorization
- **Refresh Tokens**: Long-lived (30 days) for obtaining new access tokens
- **Token Rotation**: Refresh tokens rotated on each use to prevent replay attacks
- **Token Revocation**: Support for immediate invalidation (logout, security events)

### Data Protection
- **Encryption at Rest**: Sensitive data (tokens, secrets) encrypted in Cosmos DB
- **Encryption in Transit**: TLS 1.3 required for all network communication
- **Secrets Management**: Azure Key Vault for configuration secrets
- **Token Hashing**: API keys and tokens hashed before storage

### Password Requirements
When using password-based authentication (not social login):
- Minimum 12 characters
- Mix of uppercase, lowercase, numbers, and special characters
- Password history (5 previous passwords)
- Account lockout after 5 failed attempts
- Two-factor authentication support (recommended)

### Audit Logging
Comprehensive audit trail for security and compliance:
- All authentication events (login, logout, token refresh)
- Authorization failures (attempted unauthorized access)
- Data modifications with user ID and timestamp
- Account changes (password reset, 2FA enable/disable)
- Connected account additions/removals

## Migration Strategy

### Replacing Auth0
The authentication abstraction allows straightforward provider migration:

**Step 1: Implement New Provider**
- Create implementations of core authentication interfaces
- Follow existing patterns from Auth0 implementation
- Handle provider-specific authentication flows

**Step 2: Update Configuration**
- Add new provider configuration section
- Update provider selector in configuration
- Test configuration validation

**Step 3: Service Registration**
- Create extension method for new provider
- Register provider-specific services
- Configure authentication middleware

**Step 4: Data Migration**
- Export user data from Auth0
- Map Auth0 user IDs to new provider IDs
- Import users to new authentication system
- Preserve user metadata and settings

**Step 5: Testing & Deployment**
- Test authentication flow end-to-end
- Verify token validation and user context
- Use feature flags for gradual rollout
- Monitor authentication metrics
- Maintain rollback capability

### Migration Considerations
- User ID mapping strategy (preserve or migrate)
- Token format compatibility
- Claims mapping between providers
- Social login provider differences
- MFA/2FA migration approach
- Session migration for active users

## Future Enhancements

### Planned Features
- **Social Login**: Google, GitHub, Microsoft sign-in
- **Single Sign-On (SSO)**: Enterprise SSO with SAML 2.0
- **Multi-Factor Authentication**: SMS, authenticator apps, hardware keys
- **Session Management**: View and revoke active sessions from dashboard
- **API Keys**: Generate API keys for programmatic access
- **OAuth Scopes**: Fine-grained permission scopes for third-party integrations
- **Webhooks**: Real-time notifications for authentication events
- **User Impersonation**: Support staff can impersonate users for troubleshooting

### Security Roadmap
- Passwordless authentication (WebAuthn, magic links)
- Risk-based authentication (suspicious login detection)
- IP allowlisting/blocklisting
- Device fingerprinting and management
- Security questions for account recovery
- CAPTCHA integration for bot prevention

### Compliance Features
- GDPR data export and deletion
- CCPA privacy controls
- SOC 2 audit logging
- HIPAA compliance options (if handling health data)
- Data residency controls
