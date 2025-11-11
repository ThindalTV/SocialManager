# GitHub Copilot Instructions for SocialManager

## Project Overview
SocialManager is a social media management application built with .NET 9.0, using Aspire for orchestration, Blazor for the UI, and ASP.NET Core Web API for backend services.

## Project Structure

### `/src/` - Source Code
  - `SocialManager/` - Server-side Blazor components and pages

- **Aspire/**
  - `Host/` - Aspire orchestration host for managing services
  - `ServiceDefaults/` - Shared Aspire service defaults and configuration
- **Admin/** - Main Blazor Web App with WebAssembly interactivity
  - `SocialManager.Admin/` - Blazor WebAssembly standalone admin interface
  - `SocialManager.Admin.API/` - ASP.NET Core Web API for admin operations

### `/documentation/` - Documentation
- Contains markdown files and project documentation
- Managed through `SocialManager.Documentation.csproj`
- Includes API documentation, user guides, and architecture overviews

### Copilot plans
- All copilot plans are stored under SocialManager/Plans. Each plan has its own folder with the relevant files(plan, tasks and summary). Leave the files when done.
- Each plan should be named with a prefix indicating its purpose (e.g., "Feature-", "Bugfix-", "Refactor-") followed by a descriptive title.

### `/tests/` - Unit Tests
- Location for all test projects
- Structure for unit test projects is `/tests/{ProjectName}.Tests/`.
- Each test file should correspond to a source file in the main projects.
- The files are placed in the same folder structure as the source file being tested.
- The files are Suffixed by `.Tests`.
- Use xUnit as the testing framework.

## Coding Standards

### General Guidelines
- Target framework: .NET 10.0
- Use C# nullable reference types
- Enable implicit usings where appropriate
- Follow Microsoft's C# coding conventions
- Never allow warnings.

### Blazor Components
- Use component-based architecture
- Leverage WebAssembly for interactive UI elements

### API Development
- Use minimal APIs as appropriate
- Implement proper error handling and logging
- Follow RESTful conventions for endpoints

### Aspire Integration
- Use service defaults for consistent configuration
- Leverage Aspire for service discovery and orchestration
- Configure services through the AppHost project

## Dependencies
- .NET Aspire 13.0
- ASP.NET Core 10.0
- Blazor WebAssembly

## Building and Running
- Use the Aspire AppHost project to run the entire application
- Individual projects can be run independently for development
- Restore NuGet packages before building

## Notes
- The solution uses the .slnx (XML-based) format
- All projects follow the standard .NET project structure
- Service-to-service communication should be configured through Aspire
