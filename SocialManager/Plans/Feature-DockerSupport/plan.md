# Add Docker Support for SocialManager Application

## Overview
This plan adds comprehensive Docker support to the SocialManager application, enabling containerized deployment for the .NET 10 Blazor WebAssembly and Razor Pages hybrid application with Aspire orchestration.

## Objectives
- Enable Docker containerization for the SocialManager main application
- Support multi-stage builds optimized for .NET 10
- Integrate with existing Aspire orchestration architecture
- Provide development and production Docker configurations
- Ensure proper handling of Blazor WebAssembly static assets
- Include comprehensive deployment documentation

## Steps

### 1. Create .dockerignore file in solution root
Create a `.dockerignore` file to exclude unnecessary files from the Docker build context, reducing image size and build time.

**Files to exclude:**
- Build artifacts (bin/, obj/)
- Version control (.git/, .gitignore)
- IDE files (.vs/, .vscode/)
- Documentation and test files
- Node modules and temporary files

### 2. Create Dockerfile for SocialManager main application
Create a multi-stage Dockerfile optimized for .NET 10 that:
- Uses official Microsoft .NET SDK and ASP.NET runtime images
- Builds the SocialManager application and its dependencies
- Publishes the application with optimizations
- Includes the Blazor WebAssembly client (Admin)
- Uses a non-root user for security
- Exposes appropriate ports (HTTP/HTTPS)

**Key considerations:**
- Handle project references (Admin.API, Admin, Aspire.ServiceDefaults)
- Ensure WebAssembly files are properly published
- Configure for production environment

### 3. Create docker-compose.yml for orchestration
Create a `docker-compose.yml` file that:
- Defines the SocialManager service
- Configures networking between services
- Sets up volume mounts for persistent data
- Defines service dependencies
- Exposes required ports

**Services to include:**
- SocialManager main application
- Placeholder for future services (databases, caching, etc.)

### 4. Create docker-compose.override.yml for development settings
Create development-specific overrides that:
- Enable hot reload and debugging
- Mount source code for live development
- Configure development environment variables
- Set up development ports
- Enable verbose logging

### 5. Update Aspire Host to support containerized deployment
Modify the Aspire Host configuration to:
- Support Docker container orchestration
- Add conditional logic for containerized vs. local development
- Configure container-specific service discovery
- Add Docker publishing profiles if needed

### 6. Create Docker-specific appsettings configuration
Create `appsettings.Docker.json` that:
- Configures logging for containerized environments
- Sets appropriate connection strings for Docker networking
- Configures health checks
- Sets security headers and CORS policies
- Defines container-specific URLs and ports

### 7. Add Docker build scripts for automation
Create PowerShell/bash scripts for common Docker operations:
- `docker-build.ps1` / `docker-build.sh` - Build Docker images
- `docker-run.ps1` / `docker-run.sh` - Run containers locally
- `docker-push.ps1` / `docker-push.sh` - Push to container registry
- `docker-clean.ps1` / `docker-clean.sh` - Clean up containers and images

### 8. Create Docker deployment documentation
Create comprehensive documentation covering:
- Prerequisites (Docker, Docker Compose)
- Building Docker images
- Running containers locally
- Environment variables and configuration
- Deployment to container registries (Docker Hub, Azure Container Registry)
- Production deployment considerations
- Troubleshooting common issues
- Health checks and monitoring

## Technical Details

### Target Framework
- .NET 10.0

### Architecture
- Main application: Razor Pages with Blazor WebAssembly integration
- Admin UI: Standalone Blazor WebAssembly
- API: ASP.NET Core Web API
- Orchestration: .NET Aspire

### Docker Image Strategy
- Base image: `mcr.microsoft.com/dotnet/aspnet:10.0`
- SDK image: `mcr.microsoft.com/dotnet/sdk:10.0`
- Multi-stage build for optimized size
- Non-root user for security

### Port Configuration
- HTTP: 8080 (internal)
- HTTPS: 8443 (internal)
- Aspire Dashboard: 18888 (if running Aspire Host in container)

## Success Criteria
- [ ] Docker images build successfully without errors
- [ ] Application runs in container with full functionality
- [ ] Blazor WebAssembly loads correctly from containerized app
- [ ] Razor Pages render properly
- [ ] Static assets are served correctly
- [ ] Health checks pass
- [ ] Documentation is complete and accurate
- [ ] Development workflow is smooth with docker-compose

## Dependencies
- Docker Desktop or Docker Engine (20.10+)
- Docker Compose (v2.0+)
- .NET 10.0 SDK (for local builds)

## Risks and Mitigations

### Risk: Blazor WebAssembly files not included in container
**Mitigation**: Ensure proper publish configuration and verify all _framework files are copied

### Risk: Connection issues between containerized services
**Mitigation**: Use Docker networking and proper service discovery configuration

### Risk: Performance degradation in containers
**Mitigation**: Optimize Dockerfile, use appropriate resource limits, enable production optimizations

### Risk: Configuration differences between development and production
**Mitigation**: Use environment-specific appsettings files and docker-compose overrides

## Future Enhancements
- Kubernetes deployment manifests
- Azure Container Apps deployment
- CI/CD pipeline integration
- Multi-architecture support (ARM64)
- Health monitoring and metrics collection
- Database containerization
