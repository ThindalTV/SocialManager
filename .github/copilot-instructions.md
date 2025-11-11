# GitHub Copilot Instructions for SocialManager

## Project Overview
SocialManager is a social media management application built with .NET 9.0, using Aspire for orchestration, Blazor for the UI, and ASP.NET Core Web API for backend services.

## Project Structure

### `/src/` - Source Code
- **Aspire/**
  - `Host/` - Aspire orchestration host for managing services
  - `ServiceDefaults/` - Shared Aspire service defaults and configuration
- **SocialManager/** - Main Blazor Web App with WebAssembly interactivity
  - `SocialManager/` - Server-side Blazor components and pages
  - `SocialManager.Client/` - Client-side WebAssembly components
- **SocialManager.Admin/** - Blazor WebAssembly standalone admin interface
- **SocialManager.Admin.API/** - ASP.NET Core Web API for admin operations

### `/documentation/` - Documentation
- Contains markdown files and project documentation
- Managed through `SocialManager.Documentation.csproj`

### `/tests/` - Unit Tests
- Location for all test projects

## Coding Standards

### General Guidelines
- Target framework: .NET 9.0
- Use C# nullable reference types
- Enable implicit usings where appropriate
- Follow Microsoft's C# coding conventions

### Blazor Components
- Use component-based architecture
- Separate concerns between server and client components
- Leverage WebAssembly for interactive UI elements

### API Development
- Use minimal APIs or controller-based APIs as appropriate
- Implement proper error handling and logging
- Follow RESTful conventions for endpoints

### Aspire Integration
- Use service defaults for consistent configuration
- Leverage Aspire for service discovery and orchestration
- Configure services through the AppHost project

## Dependencies
- .NET Aspire 8.2.2
- ASP.NET Core 9.0
- Blazor WebAssembly

## Building and Running
- Use the Aspire AppHost project to run the entire application
- Individual projects can be run independently for development
- Restore NuGet packages before building

## Notes
- The solution uses the .slnx (XML-based) format
- All projects follow the standard .NET project structure
- Service-to-service communication should be configured through Aspire
