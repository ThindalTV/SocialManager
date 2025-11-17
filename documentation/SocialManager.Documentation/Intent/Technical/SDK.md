# SocialManager SDK, API & Webhooks Documentation

## Overview

SocialManager exposes content and functionality through three primary integration methods, allowing developers and third-party applications to interact with the platform based on their specific requirements:

1. **SDK** - Type-safe .NET libraries for deep integration
2. **REST API** - HTTP-based endpoints for platform-agnostic access
3. **Webhooks** - Event-driven notifications for real-time updates

This document outlines how each integration method exposes content, when to use each approach, and the philosophy behind the multi-modal integration strategy.

## 1. SDK Integration

### Overview

The SocialManager SDK provides strongly-typed .NET libraries for seamless integration with .NET applications. It wraps the underlying REST API with developer-friendly interfaces, offering compile-time type safety, IntelliSense support, and automatic handling of serialization, authentication, and HTTP complexities.

### Target Frameworks
- .NET 10.0 (primary)
- .NET Standard 2.1 (for broader compatibility)

### How Content is Exposed

The SDK exposes content through **interface-based contracts** distributed across modular NuGet packages:

#### Package Structure
- **SocialManager.SDK.Core** - Base authentication, client infrastructure, and shared models
- **SocialManager.SDK.Blog** - Blog post management, publishing, and analytics
- **SocialManager.SDK.Social** - Social media post creation and cross-platform publishing
- **SocialManager.SDK.Streaming** - Live stream management and multi-platform broadcasting
- **SocialManager.SDK.Chat** - Unified chat access across streaming platforms

### Content Access Patterns

#### Pull-Based Access (Query Pattern)
The SDK provides **provider interfaces** that allow applications to retrieve content on-demand:

- **Blog Posts**: Query published posts, retrieve by ID or slug, filter by tags/categories, paginate results
- **Social Posts**: Access historical posts, filter by platform, retrieve engagement metrics
- **Stream Sessions**: Get active streams, historical session data, viewer analytics
- **Chat Messages**: Access chat history, retrieve message context

Applications can query content whenever needed, with support for:
- **Filtering** - Narrow results by status, date range, author, platform
- **Pagination** - Handle large result sets efficiently
- **Sorting** - Order results by relevance, date, popularity
- **Search** - Full-text search across content

#### Push-Based Access (Event Pattern)
The SDK provides **event handler interfaces** that notify applications when content changes occur:

- **Blog Events**: New post created, post published, post updated, post deleted
- **Social Events**: Post published, engagement received, platform-specific events
- **Streaming Events**: Stream started, viewer count changed, stream ended
- **Chat Events**: Message received, user subscribed, channel points redeemed

Applications subscribe to specific events and receive real-time notifications when content is created, modified, or deleted within SocialManager.

#### Command Pattern (Write Operations)
The SDK exposes **management interfaces** for content creation and modification:

- **Create Operations**: Draft new blog posts, schedule social posts, initiate streams
- **Update Operations**: Modify existing content, change status, update metadata
- **Delete Operations**: Remove content, unpublish posts, end streams
- **Scheduling**: Queue content for future publication with time zone support

### Authentication & Authorization

The SDK handles authentication through **pluggable authentication providers**:

- **API Key Authentication** - For server-to-server integrations
- **OAuth 2.0** - For user-delegated access
- **JWT Tokens** - For session-based authentication

Authentication is configured once during client initialization and automatically applied to all subsequent API calls. The SDK handles token refresh, expiration, and re-authentication transparently.

### Error Handling

The SDK exposes errors through **strongly-typed exception hierarchies**:

- **Authentication Exceptions** - Token expired, invalid credentials, insufficient permissions
- **Rate Limit Exceptions** - Quota exceeded, includes retry-after information
- **Validation Exceptions** - Invalid input, missing required fields
- **Not Found Exceptions** - Requested resource doesn't exist
- **Conflict Exceptions** - Resource state conflicts, concurrency issues

Applications can catch specific exception types and handle them appropriately rather than parsing HTTP status codes or error messages.

### Dependency Injection Support

The SDK is designed for **dependency injection frameworks**:

- Service registration through extension methods
- Scoped lifetime management for HTTP clients
- Configuration through options pattern
- Support for ASP.NET Core, Blazor, and console applications

### Use Cases for SDK Integration

**Choose the SDK when:**
- Building .NET applications (ASP.NET Core, Blazor, console apps, services)
- Need compile-time type safety and IntelliSense support
- Want seamless integration with dependency injection
- Prefer working with C# interfaces over HTTP endpoints
- Building backend services that manage content
- Need real-time event notifications within the application

**Examples:**
- A Blazor admin dashboard for managing blog posts
- An ASP.NET Core background service that automatically publishes content
- A console application that imports blog posts from external sources
- A .NET MAUI mobile app for content creators

---

## 2. REST API Integration

### Overview

The SocialManager REST API provides platform-agnostic HTTP endpoints following RESTful conventions. It exposes the same functionality as the SDK but through standard HTTP methods, making it accessible from any programming language or platform that supports HTTP.

### Base URL Structure

```
Production: https://api.socialmanager.io/v1
Staging: https://api-staging.socialmanager.io/v1
```

### How Content is Exposed

Content is exposed through **resource-oriented URLs** organized by feature area:

#### Blog Content
- `/v1/blog/posts` - Blog post collection
- `/v1/blog/posts/{id}` - Individual blog post
- `/v1/blog/posts/{id}/publish` - Publishing actions
- `/v1/blog/posts/{id}/schedule` - Scheduling operations

#### Social Content
- `/v1/social/posts` - Social post collection
- `/v1/social/posts/multi-platform` - Cross-platform publishing
- `/v1/social/posts/{id}` - Individual social post

#### Streaming Content
- `/v1/streaming/sessions` - Stream session collection
- `/v1/streaming/sessions/{id}` - Individual stream session
- `/v1/streaming/platforms` - Platform configuration

#### Analytics Content
- `/v1/analytics/blog/posts/{id}/metrics` - Blog post performance data
- `/v1/analytics/social/posts/{id}/metrics` - Social post engagement
- `/v1/analytics/streaming/sessions/{id}/metrics` - Stream analytics

### Content Retrieval Methods

#### GET Requests (Read Operations)
Retrieve content without modifying server state:

- **List Resources**: Paginated collections with filtering and sorting
- **Get Single Resource**: Retrieve complete resource by unique identifier
- **Query Parameters**: Filter, sort, paginate, and search results
- **Conditional Requests**: Support for ETags and If-Modified-Since headers

#### POST Requests (Create Operations)
Create new resources:

- **Create Content**: Submit new blog posts, social posts, stream sessions
- **Action Endpoints**: Trigger specific actions (publish, schedule, start stream)
- **Batch Operations**: Create multiple resources in single request

#### PUT Requests (Update Operations)
Modify existing resources:

- **Full Updates**: Replace entire resource
- **Status Changes**: Modify publication state
- **Metadata Updates**: Change tags, categories, settings

#### DELETE Requests (Remove Operations)
Remove resources:

- **Hard Delete**: Permanently remove content
- **Soft Delete**: Mark as deleted but preserve data
- **Cascade Options**: Handle related content

### Authentication Methods

The REST API supports multiple authentication mechanisms:

- **API Key**: Passed in `Authorization: ApiKey {key}` header
- **OAuth 2.0**: Passed in `Authorization: Bearer {token}` header
- **JWT**: Passed in `Authorization: Bearer {jwt}` header

All authentication methods provide the same level of access based on configured scopes.

### Content Formats

All content is exchanged in **JSON format**:

- Request bodies use `Content-Type: application/json`
- Response bodies use `Content-Type: application/json`
- Date/time values use ISO 8601 format
- Pagination metadata included in responses
- Error messages follow consistent structure

### Rate Limiting

The API enforces **per-tier rate limits** to ensure fair usage:

- **Free Tier**: 100 requests/hour
- **Basic Tier**: 1,000 requests/hour
- **Pro Tier**: 10,000 requests/hour
- **Enterprise**: Custom limits

Rate limit information exposed through response headers:
- `X-RateLimit-Limit` - Maximum requests allowed
- `X-RateLimit-Remaining` - Requests remaining in current window
- `X-RateLimit-Reset` - Unix timestamp when limit resets

### Versioning Strategy

The API uses **URL-based versioning** (`/v1/`, `/v2/`) to ensure backward compatibility:

- Version specified in URL path
- Breaking changes require new version
- Non-breaking changes added to existing version
- Deprecated versions supported for minimum 12 months

### Use Cases for REST API Integration

**Choose the REST API when:**
- Building non-.NET applications (Python, Node.js, Ruby, PHP, Go)
- Developing mobile applications (iOS, Android, React Native)
- Creating frontend applications (React, Vue, Angular)
- Integrating from serverless functions or microservices
- Need direct control over HTTP requests
- Working in environments without SDK support

**Examples:**
- A React dashboard that displays blog analytics
- A Python script that imports content from external sources
- An iOS mobile app for content creators
- A serverless function that processes content on schedule
- A third-party application integrating SocialManager features

---

## 3. Webhook Integration

### Overview

Webhooks provide **push-based, event-driven notifications** by sending HTTP POST requests to registered endpoints when specific events occur in SocialManager. This eliminates the need for polling and enables real-time reactions to content changes.

### How Content is Exposed

Content changes are exposed through **event payloads** delivered to subscriber endpoints:

#### Event Registration
Applications register webhook endpoints by:

1. **Specifying Target URL**: HTTPS endpoint that will receive events
2. **Selecting Events**: Choose specific event types to receive
3. **Providing Secret**: Shared secret for payload signature verification
4. **Configuring Options**: Set active/inactive status, filters

#### Event Delivery
When subscribed events occur, SocialManager:

1. **Generates Event Payload**: Constructs JSON payload with event data
2. **Signs Payload**: Creates HMAC-SHA256 signature for verification
3. **Sends HTTP POST**: Delivers to registered endpoint with signature header
4. **Implements Retry Logic**: Retries failed deliveries with exponential backoff

#### Event Payload Structure
All events follow consistent structure:

- **Event ID**: Unique identifier for deduplication
- **Event Type**: Categorized event name (e.g., `blog.post.published`)
- **Timestamp**: When event occurred (ISO 8601)
- **Data**: Event-specific content and metadata

### Available Event Categories

#### Blog Events
Notifies subscribers of blog content lifecycle changes:

- Post created (draft)
- Post published
- Post updated
- Post deleted
- Post scheduled for publication

#### Social Events
Notifies subscribers of social media activity:

- Social post published
- Social post deleted
- Multi-platform post completed
- Post scheduled

#### Streaming Events
Notifies subscribers of live stream state changes:

- Stream started
- Stream ended
- Stream settings updated
- Viewer milestone reached

#### Chat Events
Notifies subscribers of chat activity across platforms:

- Chat message received
- User subscribed
- Channel points redeemed
- Raid or host notification

#### Analytics Events
Notifies subscribers of metric milestones:

- Page view milestone (10k, 100k, 1M views)
- Engagement threshold reached
- Traffic spike detected

### Security & Verification

Webhooks implement **signature-based verification**:

- Every webhook includes `X-SocialManager-Signature` header
- Signature calculated using HMAC-SHA256 with shared secret
- Consumers must verify signature before processing payload
- Prevents unauthorized or tampered webhook deliveries

### Delivery Guarantees

Webhooks implement **at-least-once delivery**:

- **Immediate Delivery**: Attempt delivery within seconds of event
- **Retry Logic**: 5 retry attempts with exponential backoff (1s, 5s, 30s, 5m, 1h)
- **Failure Tracking**: Failed webhooks logged for manual investigation
- **Idempotency**: Consumers should handle duplicate deliveries using event IDs

### Webhook Management

Subscribers can manage webhooks through API endpoints:

- **Create**: Register new webhook endpoint
- **List**: View all registered webhooks
- **Update**: Modify event subscriptions or URL
- **Delete**: Remove webhook registration
- **Test**: Trigger test event delivery

### Use Cases for Webhook Integration

**Choose Webhooks when:**
- Need real-time notifications of content changes
- Building automation workflows
- Integrating with external systems that react to events
- Reducing API polling and server load
- Implementing event-driven architectures
- Connecting to automation platforms (Zapier, IFTTT-style)

**Examples:**
- Automatically tweet when blog post is published
- Send email notification when stream starts
- Update external database when content changes
- Trigger CI/CD pipeline when configuration updates
- Post to Slack channel when milestone reached
- Update analytics dashboard in real-time

---

## Integration Strategy & Selection Guide

### When to Use Each Approach

#### SDK (Pull + Push + Command)
**Best for:**
- .NET applications requiring deep integration
- Applications that both read and write content
- Real-time event handling within application
- Strongly-typed, compile-time safe code

**Access Pattern:** Bidirectional - query content, receive events, execute commands

#### REST API (Pull + Command)
**Best for:**
- Non-.NET applications and platforms
- Simple content retrieval or publishing
- Mobile and web frontend applications
- Stateless, request-response workflows

**Access Pattern:** Request-driven - explicit HTTP requests for data or actions

#### Webhooks (Push Only)
**Best for:**
- Real-time notifications without polling
- Event-driven automation and workflows
- Reacting to changes in external systems
- Reducing API call volume

**Access Pattern:** Event-driven - passive reception of notifications

### Combined Usage Patterns

These approaches are **complementary and designed to work together**:

#### Pattern 1: SDK + Webhooks
- Use SDK for content management within .NET application
- Use webhooks to trigger external systems when content changes
- Example: Admin dashboard (SDK) triggers notification service (webhooks)

#### Pattern 2: REST API + Webhooks
- Use REST API from frontend application to display content
- Use webhooks to push updates to backend service
- Example: React dashboard (REST API) with real-time updates (webhooks)

#### Pattern 3: SDK + REST API
- Use SDK in backend .NET services for content management
- Expose REST API to non-.NET consumers (mobile apps, web)
- Example: ASP.NET Core backend (SDK) serving mobile apps (REST API)

#### Pattern 4: All Three
- SDK for backend content orchestration
- REST API for frontend and mobile access
- Webhooks for real-time notifications and automation
- Example: Full-stack application with event-driven automation

---

## Content Exposure Philosophy

### Security-First Design

All integration methods implement **defense-in-depth security**:

- **Authentication Required**: No anonymous access to content APIs
- **Authorization Scoping**: Granular permissions per integration method
- **Transport Security**: HTTPS required for all communications
- **Signature Verification**: Webhooks include cryptographic signatures
- **Rate Limiting**: Prevents abuse and ensures fair usage
- **Audit Logging**: All API access logged for security monitoring

### Consistency Across Methods

The same underlying content is accessible through all three methods:

- **Same Resources**: Blog posts, social posts, streams accessible via SDK, API, webhooks
- **Same Permissions**: Authorization rules apply consistently
- **Same Data**: No feature disparity between integration methods
- **Same Versioning**: API versions respected across all methods

### API-First Development

SocialManager follows **API-first design principles**:

- REST API is primary interface for all functionality
- SDK wraps REST API with type-safe abstractions
- Webhooks deliver same data available via API
- Internal systems use same APIs as external integrators
- Documentation generated from API specifications

### Versioning & Backward Compatibility

All integration methods use **explicit versioning**:

- URL-based API versions (`/v1/`, `/v2/`)
- SDK packages versioned independently
- Breaking changes require new major version
- Deprecated versions supported for minimum 12 months
- Clear migration guides for version upgrades

---

## Authentication & Authorization

### Authentication Methods

SocialManager supports three authentication mechanisms across all integration methods:

#### API Key Authentication
**Use for:** Server-to-server integrations, background services, automation

- Long-lived credentials
- Suitable for backend systems
- Managed through admin interface
- Can be scoped to specific permissions

#### OAuth 2.0
**Use for:** User-delegated access, third-party applications

- User authorizes application access
- Supports authorization code flow
- Includes refresh token for long-term access
- Follows standard OAuth 2.0 specification

#### JWT Tokens
**Use for:** Session-based authentication, user-specific operations

- Short-lived session tokens
- Issued after user login
- Includes user identity and claims
- Validates against token signing key

### Authorization Scopes

Fine-grained permission scopes control access:

- `blog:read` - Read blog posts
- `blog:write` - Create and update blog posts
- `social:read` - Read social posts
- `social:write` - Create and delete social posts
- `streaming:read` - Read stream information
- `streaming:write` - Start and manage streams
- `analytics:read` - Access analytics data
- `webhooks:manage` - Manage webhook subscriptions

Applications request only necessary scopes, following principle of least privilege.

---

## Best Practices

### General Integration Practices

1. **Use HTTPS Everywhere**: All communications must use TLS/SSL
2. **Implement Retry Logic**: Handle transient failures with exponential backoff
3. **Cache Responses**: Reduce API calls by caching GET responses
4. **Monitor Rate Limits**: Track usage and implement client-side throttling
5. **Log All Interactions**: Maintain audit trail of API usage
6. **Version Pinning**: Explicitly specify API version in production

### Security Practices

1. **Rotate Credentials**: Change API keys every 90 days
2. **Use Least Privilege**: Request minimum necessary scopes
3. **Verify Webhook Signatures**: Always validate webhook authenticity
4. **Store Secrets Securely**: Use secret management systems (Azure Key Vault)
5. **Monitor for Anomalies**: Alert on unusual API usage patterns
6. **Implement Rate Limiting**: Protect your own endpoints receiving webhooks

### Performance Practices

1. **Connection Pooling**: Reuse HTTP connections
2. **Paginate Large Results**: Don't retrieve all data at once
3. **Use Async Operations**: Don't block threads waiting for responses
4. **Batch Operations**: Group multiple operations when supported
5. **Enable Compression**: Use gzip for request/response bodies
6. **Monitor Latency**: Track response times and set SLA alerts

---

## Support & Resources

### Documentation
- **API Reference**: https://docs.socialmanager.io/api
- **SDK Documentation**: https://docs.socialmanager.io/sdk
- **Webhooks Guide**: https://docs.socialmanager.io/webhooks
- **Authentication Guide**: https://docs.socialmanager.io/auth

### Developer Tools
- **NuGet Packages**: Official SDK packages on NuGet.org
- **Postman Collection**: Pre-configured API requests
- **Sample Applications**: Reference implementations on GitHub
- **Webhook Testing**: Tools for local webhook development

### Support Channels
- **Email**: api-support@socialmanager.io
- **Discord**: Developer community server
- **Stack Overflow**: Tag `socialmanager-api`
- **GitHub Issues**: SDK and documentation feedback

### Service Status
- **Status Page**: Real-time API availability
- **Incident History**: Past outages and resolutions
- **Maintenance Schedule**: Planned downtime notifications

---

## Changelog

### Version 1.0 (Current)
- Initial release of SDK, REST API, and Webhooks
- Support for Blog, Social, Streaming, and Chat features
- OAuth 2.0, API Key, and JWT authentication
- Webhook event system with signature verification
- Analytics endpoints for content performance

### Roadmap

#### Version 1.1 (Planned)
- GraphQL API support for flexible querying
- Improved webhook filtering and transformation
- Additional analytics endpoints
- Performance optimizations

#### Version 1.2 (Planned)
- WebSocket support for real-time updates
- Bulk operations for content management
- Enhanced error reporting and debugging
- Extended rate limit tiers

#### Version 2.0 (Future)
- Breaking changes for improved API consistency
- Deprecation of legacy endpoints
- New authentication methods
- Enhanced security features

---

## Summary

SocialManager provides **three complementary integration approaches**, each optimized for different use cases:

### SDK Integration
**Exposes content through:** Type-safe .NET interfaces with event subscriptions
**Best for:** .NET applications requiring deep, bidirectional integration
**Key benefit:** Compile-time safety and seamless .NET ecosystem integration

### REST API Integration
**Exposes content through:** HTTP endpoints following RESTful conventions
**Best for:** Platform-agnostic access from any programming language
**Key benefit:** Universal compatibility and simplicity

### Webhook Integration
**Exposes content through:** Push-based event notifications
**Best for:** Real-time reactions to content changes
**Key benefit:** Eliminates polling and enables event-driven architectures

All three methods access the same underlying content with consistent security, versioning, and authorization. Developers can choose one or combine multiple approaches based on their specific requirements, ensuring maximum flexibility for any integration scenario.
