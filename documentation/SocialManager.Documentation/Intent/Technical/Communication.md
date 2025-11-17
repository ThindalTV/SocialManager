# Communication Architecture

## Overview

SocialManager implements a decoupled architecture that separates the user interface from backend services through a well-defined API layer. This design enables multiple UI implementations (web, mobile, desktop, or CLI) to interact with the same backend services, promoting flexibility and maintainability.

## Architecture Components

### 1. Blazor WebAssembly UI

The primary user interface is built with **Blazor WebAssembly**, a client-side web framework that runs entirely in the browser using WebAssembly.

**Key Characteristics:**
- Runs in the browser's WebAssembly runtime
- No server dependency after initial download
- Can be deployed to any static file host (Azure Static Web Apps, CDN, etc.)
- Uses HttpClient to communicate with backend APIs
- Configured with service discovery for automatic endpoint resolution

### 2. Backend API

The backend is an **ASP.NET Core Web API** that provides RESTful endpoints for all business logic and data operations.

**Key Characteristics:**
- Handles all business logic and data access
- Provides RESTful HTTP endpoints
- Includes OpenAPI/Swagger documentation
- Built-in health monitoring endpoints
- Integrates with Azure Cosmos DB for data persistence

### 3. .NET Aspire Orchestration

**Aspire** handles service orchestration, configuration, and local development coordination between the UI and API.

**Key Responsibilities:**
- Automatic service discovery between components
- Manages dependencies (Cosmos DB, Azure Storage)
- Provides unified dashboard for monitoring
- Simplifies local development environment

## Communication Flow

```
User Browser
    ?
Blazor WebAssembly App
    ?
HttpClient (with service discovery)
    ?
HTTP/HTTPS Request
    ?
ASP.NET Core Web API
    ?
Business Logic Services
    ?
Data Layer (Cosmos DB)
    ?
HTTP Response (JSON)
    ?
Blazor Component Updates UI
```

### Request/Response Lifecycle:

1. **User Interaction** - User clicks a button or submits a form
2. **Service Call** - Blazor component invokes HttpClient
3. **HTTP Request** - Request sent to API with authentication headers
4. **API Processing** - Endpoint executes business logic and data operations
5. **HTTP Response** - JSON response returned with status code
6. **UI Update** - Component deserializes response and updates display

## Why This Enables Multiple UIs

### API-First Design

The backend API is completely independent of any specific UI technology:
- No UI logic in the API layer
- Standard HTTP/REST protocols
- Language-agnostic JSON data format
- OpenAPI documentation provides machine-readable contracts

### Any Client Can Connect

Because the API uses standard protocols, you can create clients using any technology:

**Mobile Apps** - .NET MAUI, React Native, Flutter  
**Desktop Apps** - WPF, WinForms, Electron  
**CLI Tools** - Console applications, PowerShell scripts  
**Third-Party Integrations** - JavaScript, Python, any HTTP-capable language

### Security at the API Layer

Authentication and authorization are enforced at the API level, not in the UI:
- All clients authenticate the same way
- Permissions verified server-side
- UI only presents the authentication flow

## HttpClient Patterns

### Named Clients
Services register named HttpClient instances with specific configurations (base URL, timeouts, retry policies). Components inject `IHttpClientFactory` to create configured clients.

### Typed Clients
For larger applications, create dedicated client classes that encapsulate API calls. These are registered as services and injected directly into components.

## Security Considerations

**CORS** - Configure Cross-Origin Resource Sharing when UI and API are on different domains

**Authentication** - Typically uses JWT tokens:
- UI obtains token after successful login
- Token stored securely (session storage, secure cookies)
- Included in Authorization header for subsequent requests

## Performance Optimization

**API Response Caching** - Cache responses for static or infrequently changing data

**UI-Side Caching** - Store API responses temporarily to avoid repeated calls

**Pagination** - Return large datasets in pages rather than all at once

## Real-Time Communication

While the current architecture uses request/response HTTP, **SignalR** can be added for real-time features like notifications, chat, or live updates. SignalR maintains persistent connections between client and server for bidirectional communication.

## Summary

The SocialManager architecture provides **clean separation of concerns**:

**Blazor WebAssembly UI** - Rich, interactive web experience running in the browser  
**ASP.NET Core Web API** - Business logic, data access, and security  
**.NET Aspire** - Service orchestration and configuration management

**Architecture Benefits:**
- ? **Flexibility** - Any HTTP-capable client can use the API
- ? **Scalability** - UI and API scale independently
- ? **Maintainability** - UI changes don't affect API and vice versa
- ? **Security** - Enforcement at the API layer
- ? **Testability** - Components tested independently

By using standard HTTP/REST principles and modern .NET patterns, the application supports multiple client types both now and in the future.
