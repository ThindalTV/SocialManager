# .NET 10.0 Upgrade Plan for SocialManager

## 1. Executive Summary

### Scenario
Upgrade the SocialManager solution from a mixed .NET 8.0/9.0 environment to .NET 10.0.

### Scope
- **Total Projects**: 6 projects
- **Current State**: Mixed target frameworks (net8.0 and net9.0)
- **Target State**: All projects targeting net10.0

### Selected Strategy
**Big Bang Strategy** - All projects will be upgraded simultaneously in a single atomic operation.

**Rationale**: 
- Small solution (6 projects)
- Simple dependency structure
- Clear upgrade path with well-defined package updates
- No blocking compatibility issues identified
- All required packages have .NET 10.0 compatible versions available

### Complexity Assessment
**Medium** - The solution has a straightforward structure with minimal complexity:
- Most projects already on .NET 9.0, only 2 projects need upgrade from .NET 8.0
- Well-defined package dependencies
- No circular dependencies
- Clear dependency chain

### Critical Issues
**Deprecated Packages** (must be addressed):
- `Aspire.Hosting.AppHost` 8.2.2 - Out of support, must upgrade to 13.0.0
- `Microsoft.Extensions.ServiceDiscovery` 8.2.2 - Out of support, must upgrade to 10.0.0

### Recommended Approach
Big Bang migration with all projects upgraded in a single pass, followed by comprehensive testing.

---

## 2. Migration Strategy

### 2.1 Approach Selection

**Chosen Strategy**: Big Bang Strategy

**Justification**:
- **6 projects total** - Small enough for simultaneous upgrade
- **Simple dependency graph** - Linear dependencies with no complex relationships
- **Minimal codebase** - Total of 400 LOC across all projects
- **Homogeneous stack** - All ASP.NET Core/Blazor/Aspire projects
- **Clear package upgrade path** - All packages have known .NET 10.0 versions

### 2.2 Dependency-Based Ordering

The dependency structure is straightforward:
```
SocialManager.Aspire.Host (net8.0)
  └── SocialManager (net9.0)
      ├── SocialManager.Admin (net9.0)
      ├── SocialManager.Admin.API (net9.0)
      └── SocialManager.Aspire.ServiceDefaults (net8.0)

SocialManager.Documentation (net9.0) - Standalone
```

**Migration Order**: Due to Big Bang strategy, all projects will be upgraded simultaneously. However, the logical dependency order is:
1. **Foundation Layer**: ServiceDefaults (no dependencies)
2. **Mid-tier**: Admin, Admin.API (depend on ServiceDefaults via SocialManager)
3. **Application Layer**: SocialManager (orchestrates everything)
4. **Host Layer**: Aspire.Host (entry point)
5. **Documentation**: Independent documentation project

### 2.3 Parallel vs Sequential Execution

**Strategy**: All projects updated in parallel as a single atomic operation per Big Bang strategy.

---

## 3. Detailed Dependency Analysis

### 3.1 Dependency Graph Summary

**Phase 0**: Prerequisites and environment setup
**Phase 1**: Atomic upgrade of all 6 projects simultaneously
**Phase 2**: Build verification and compilation error fixes
**Phase 3**: Testing and validation

### 3.2 Project Groupings

#### Phase 0: Preparation
- Verify .NET 10.0 SDK installation
- Commit any pending changes
- Ensure working directory is clean

#### Phase 1: Atomic Upgrade (All Projects)
**All projects updated simultaneously**:
- SocialManager.Aspire.ServiceDefaults (net8.0 → net10.0)
- SocialManager.Admin (net9.0 → net10.0)
- SocialManager.Admin.API (net9.0 → net10.0)
- SocialManager (net9.0 → net10.0)
- SocialManager.Aspire.Host (net8.0 → net10.0)
- SocialManager.Documentation (net9.0 → net10.0)

---

## 4. Project-by-Project Migration Plans

### Project: SocialManager.Aspire.ServiceDefaults

**Current State**
- Target Framework: net8.0
- Dependencies: 0 project dependencies
- Dependants: SocialManager
- Package Count: 7
- LOC: 118

**Target State**
- Target Framework: net10.0
- Updated Packages: 4

**Migration Steps**

1. **Prerequisites**
   - None - this is a leaf node project

2. **Framework Update**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | Microsoft.Extensions.Http.Resilience | 8.10.0 | 10.0.0 | Framework compatibility |
   | Microsoft.Extensions.ServiceDiscovery | 8.2.2 | 10.0.0 | **DEPRECATED - Out of support** |
   | OpenTelemetry.Instrumentation.AspNetCore | 1.9.0 | 1.14.0-rc.1 | Framework compatibility |
   | OpenTelemetry.Instrumentation.Http | 1.9.0 | 1.14.0-rc.1 | Framework compatibility |

   **Compatible packages (no update needed)**:
   - OpenTelemetry.Exporter.OpenTelemetryProtocol 1.9.0
   - OpenTelemetry.Extensions.Hosting 1.9.0
   - OpenTelemetry.Instrumentation.Runtime 1.9.0

4. **Expected Breaking Changes**
   - Service Discovery API changes due to major version upgrade (8.2.2 → 10.0.0)
   - Potential telemetry configuration changes with OpenTelemetry RC packages
   - Resilience policy configuration may have new patterns

5. **Code Modifications**
   - Review `Extensions.cs` for any obsolete API usage
   - Update service discovery registration patterns if needed
   - Verify OpenTelemetry instrumentation configuration

6. **Testing Strategy**
   - Unit tests: Verify extension methods work correctly
   - Integration tests: Ensure service discovery and telemetry function
   - Manual testing: Check OpenTelemetry traces and metrics

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] No security warnings

---

### Project: SocialManager.Admin

**Current State**
- Target Framework: net9.0
- Dependencies: 0 project dependencies
- Dependants: SocialManager
- Package Count: 3
- LOC: 33

**Target State**
- Target Framework: net10.0
- Updated Packages: 3

**Migration Steps**

1. **Prerequisites**
   - None - this is a leaf node project

2. **Framework Update**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | Microsoft.AspNetCore.Components.Web | 9.0.10 | 10.0.0 | Framework compatibility |
   | Microsoft.AspNetCore.Components.WebAssembly | 9.0.10 | 10.0.0 | Framework compatibility |
   | Microsoft.AspNetCore.Components.WebAssembly.Server | 9.0.10 | 10.0.0 | Framework compatibility |

4. **Expected Breaking Changes**
   - Blazor WebAssembly component lifecycle changes
   - Potential changes in component parameter handling
   - JavaScript interop patterns may have updates

5. **Code Modifications**
   - Review Razor components for obsolete APIs
   - Update component lifecycle methods if needed
   - Check JavaScript interop calls in `exampleJsInterop.js`

6. **Testing Strategy**
   - Manual testing: Verify all pages render correctly (Dashboard, Posts, Settings)
   - Manual testing: Test navigation and layout components
   - Manual testing: Verify CSS styling remains intact

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Application starts correctly
   - [ ] All pages render without errors
   - [ ] No security warnings

---

### Project: SocialManager.Admin.API

**Current State**
- Target Framework: net9.0
- Dependencies: 0 project dependencies
- Dependants: SocialManager
- Package Count: 1
- LOC: 41

**Target State**
- Target Framework: net10.0
- Updated Packages: 1

**Migration Steps**

1. **Prerequisites**
   - None - this is a leaf node project

2. **Framework Update**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | Microsoft.AspNetCore.OpenApi | 9.0.10 | 10.0.0 | Framework compatibility |

4. **Expected Breaking Changes**
   - OpenAPI/Swagger configuration patterns may change
   - Minimal API endpoint configuration updates
   - Authentication/authorization middleware changes

5. **Code Modifications**
   - Review `Program.cs` for obsolete API patterns
   - Update OpenAPI configuration if needed
   - Verify middleware pipeline configuration

6. **Testing Strategy**
   - Manual testing: Test API endpoints via .http file
   - Manual testing: Verify Swagger UI loads correctly
   - Manual testing: Check authentication/authorization

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] API starts correctly
   - [ ] Swagger/OpenAPI documentation generates
   - [ ] No security warnings

---

### Project: SocialManager

**Current State**
- Target Framework: net9.0
- Dependencies: 3 (Admin, Admin.API, ServiceDefaults)
- Dependants: Aspire.Host
- Package Count: 0 explicit packages
- LOC: 205

**Target State**
- Target Framework: net10.0
- Updated Packages: 0 (depends on referenced projects)

**Migration Steps**

1. **Prerequisites**
   - ServiceDefaults, Admin, and Admin.API must be upgraded (done simultaneously in Big Bang)

2. **Framework Update**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

3. **Package Updates**
   - No explicit package references
   - Transitive dependencies will be updated through project references

4. **Expected Breaking Changes**
   - Blazor Server/WebAssembly hybrid hosting changes
   - Component rendering mode changes
   - Static asset handling updates

5. **Code Modifications**
   - Review Blazor components for framework changes
   - Update routing configuration if needed
   - Verify component interactivity modes

6. **Testing Strategy**
   - Manual testing: Verify application starts
   - Manual testing: Test all pages and components
   - Manual testing: Verify admin integration works

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Application starts correctly
   - [ ] All features functional
   - [ ] No security warnings

---

### Project: SocialManager.Aspire.Host

**Current State**
- Target Framework: net8.0
- Dependencies: 1 (SocialManager)
- Dependants: 0 (entry point)
- Package Count: 1
- LOC: 3

**Target State**
- Target Framework: net10.0
- Updated Packages: 1

**Migration Steps**

1. **Prerequisites**
   - SocialManager must be upgraded (done simultaneously in Big Bang)

2. **Framework Update**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | Aspire.Hosting.AppHost | 8.2.2 | 13.0.0 | **DEPRECATED - Out of support, major version upgrade** |

4. **Expected Breaking Changes**
   - **MAJOR**: Aspire hosting model changes (8.x → 13.x is a major jump)
   - Service registration patterns may have changed
   - Resource configuration API changes
   - Dashboard integration changes

5. **Code Modifications**
   - **CRITICAL**: Review `Program.cs` - the Aspire API has likely changed significantly
   - Update resource registration calls
   - Verify service discovery configuration
   - Check dashboard integration

6. **Testing Strategy**
   - Manual testing: Verify Aspire dashboard launches
   - Manual testing: Ensure all services are discovered
   - Manual testing: Check telemetry and logging
   - Integration testing: Verify service-to-service communication

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Aspire host starts correctly
   - [ ] All services are orchestrated properly
   - [ ] Dashboard accessible and functional
   - [ ] No security warnings

---

### Project: SocialManager.Documentation

**Current State**
- Target Framework: net9.0
- Dependencies: 0
- Dependants: 0 (standalone)
- Package Count: 0
- LOC: 0

**Target State**
- Target Framework: net10.0
- Updated Packages: 0

**Migration Steps**

1. **Prerequisites**
   - None - standalone project

2. **Framework Update**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

3. **Package Updates**
   - No packages to update

4. **Expected Breaking Changes**
   - None expected (documentation project)

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Verify project builds

7. **Validation Checklist**
   - [ ] Project builds without errors
   - [ ] Project builds without warnings

---

## 5. Package Update Reference

### Common Package Updates

| Package | Current | Target | Projects Affected | Update Reason |
|---------|---------|--------|-------------------|---------------|
| Microsoft.AspNetCore.Components.Web | 9.0.10 | 10.0.0 | 1 (Admin) | .NET 10.0 compatibility |
| Microsoft.AspNetCore.Components.WebAssembly | 9.0.10 | 10.0.0 | 1 (Admin) | .NET 10.0 compatibility |
| Microsoft.AspNetCore.Components.WebAssembly.Server | 9.0.10 | 10.0.0 | 1 (Admin) | .NET 10.0 compatibility |
| Microsoft.AspNetCore.OpenApi | 9.0.10 | 10.0.0 | 1 (Admin.API) | .NET 10.0 compatibility |
| Microsoft.Extensions.Http.Resilience | 8.10.0 | 10.0.0 | 1 (ServiceDefaults) | .NET 10.0 compatibility |

### Critical Updates (Deprecated/Security)

| Package | Current | Target | Projects Affected | Update Reason |
|---------|---------|--------|-------------------|---------------|
| **Aspire.Hosting.AppHost** | **8.2.2** | **13.0.0** | 1 (Aspire.Host) | **OUT OF SUPPORT - Security and maintenance** |
| **Microsoft.Extensions.ServiceDiscovery** | **8.2.2** | **10.0.0** | 1 (ServiceDefaults) | **OUT OF SUPPORT - Security and maintenance** |

### Preview/RC Packages

| Package | Current | Target | Projects Affected | Update Reason |
|---------|---------|--------|-------------------|---------------|
| OpenTelemetry.Instrumentation.AspNetCore | 1.9.0 | 1.14.0-rc.1 | 1 (ServiceDefaults) | .NET 10.0 compatibility |
| OpenTelemetry.Instrumentation.Http | 1.9.0 | 1.14.0-rc.1 | 1 (ServiceDefaults) | .NET 10.0 compatibility |

---

## 6. Breaking Changes Catalog

### .NET 10.0 Framework Breaking Changes

1. **Blazor Changes**
   - Component rendering modes may have new defaults
   - WebAssembly interop patterns may be updated
   - Static asset handling changes

2. **ASP.NET Core Changes**
   - Minimal API hosting model updates
   - Middleware pipeline configuration changes
   - OpenAPI/Swagger integration updates

3. **Aspire Changes** (8.2.2 → 13.0.0)
   - **MAJOR VERSION JUMP**: Significant API changes expected
   - Resource registration patterns likely changed
   - Service discovery configuration updates
   - Dashboard integration changes

### Package-Specific Breaking Changes

1. **Aspire.Hosting.AppHost 8.2.2 → 13.0.0**
   - Expected: Major API redesign
   - Review: All `Program.cs` code in Aspire.Host
   - Action: Consult migration guide for Aspire 13.0

2. **Microsoft.Extensions.ServiceDiscovery 8.2.2 → 10.0.0**
   - Expected: API changes in service registration
   - Review: ServiceDefaults `Extensions.cs`
   - Action: Update service discovery patterns

3. **OpenTelemetry RC Packages**
   - Expected: Instrumentation configuration changes
   - Review: Telemetry setup in ServiceDefaults
   - Action: Verify tracing and metrics still work

---

## 7. Risk Management

### 7.1 High-Risk Changes

| Project | Risk | Mitigation |
|---------|------|------------|
| **SocialManager.Aspire.Host** | **HIGH** - Aspire major version jump (8→13) | Thorough testing, review Aspire 13.0 migration docs, manual verification of all orchestration |
| **SocialManager.Aspire.ServiceDefaults** | **MEDIUM** - Service Discovery deprecation, OpenTelemetry RC | Careful testing of service discovery, verify telemetry collection |
| **SocialManager.Admin** | **MEDIUM** - Blazor WebAssembly updates | Manual UI testing of all pages and components |
| SocialManager.Admin.API | LOW - Simple API with one package | Standard testing of API endpoints |
| SocialManager | LOW - No direct package changes | Integration testing |
| SocialManager.Documentation | LOW - Empty documentation project | Build verification only |

### 7.2 Contingency Plans

**If Aspire 13.0 has breaking changes that block the upgrade:**
- Fallback: Temporarily use compatibility mode if available
- Alternative: Investigate Aspire 12.x as intermediate step
- Last resort: Revert upgrade and plan phased migration

**If Service Discovery changes break functionality:**
- Fallback: Use explicit service configuration temporarily
- Research: Review .NET 10.0 service discovery patterns
- Fix: Update service registration code accordingly

**If OpenTelemetry RC packages have stability issues:**
- Fallback: Remain on stable 1.9.0 packages initially
- Monitor: Track for stable 1.14.0 release
- Upgrade: Move to stable version when available

---

## 8. Testing and Validation Strategy

### 8.1 Phase-by-Phase Testing

#### Phase 0: Prerequisites
- [ ] .NET 10.0 SDK installed and verified
- [ ] Working directory clean
- [ ] All changes committed

#### Phase 1: Atomic Upgrade Completion
- [ ] All 6 project files updated to net10.0
- [ ] All packages updated to target versions
- [ ] Solution restores without errors

#### Phase 2: Build Verification
- [ ] Entire solution builds without errors
- [ ] Entire solution builds without warnings
- [ ] No dependency conflicts reported

#### Phase 3: Functional Testing
- [ ] Aspire Host starts successfully
- [ ] Aspire Dashboard accessible
- [ ] SocialManager application starts
- [ ] Admin interface loads and renders
- [ ] Admin API responds to requests
- [ ] Service discovery functioning
- [ ] Telemetry collection working

### 8.2 Smoke Tests

**After atomic upgrade:**
1. **Build Test**
   ```bash
   dotnet build SocialManager.slnx --no-incremental
   ```
   Expected: Success with 0 errors, 0 warnings

2. **Restore Test**
   ```bash
   dotnet restore SocialManager.slnx
   ```
   Expected: All packages restored successfully

3. **Run Test**
   ```bash
   dotnet run --project src/Aspire/SocialManager.Aspire.Host/SocialManager.Aspire.Host.csproj
   ```
   Expected: Application starts, Aspire dashboard loads

### 8.3 Comprehensive Validation

**Before marking upgrade complete:**

1. **Build Validation**
   - [ ] Clean build succeeds: `dotnet clean && dotnet build`
   - [ ] No warnings in build output
   - [ ] All projects target net10.0

2. **Runtime Validation**
   - [ ] Aspire Host launches without errors
   - [ ] Aspire Dashboard accessible at expected URL
   - [ ] All registered services shown in dashboard
   - [ ] Health checks pass

3. **Application Validation**
   - [ ] SocialManager app loads
   - [ ] Navigation works
   - [ ] Static assets load correctly

4. **Admin Interface Validation**
   - [ ] Admin UI loads
   - [ ] Dashboard page renders
   - [ ] Posts page renders
   - [ ] Settings page renders
   - [ ] Navigation functions
   - [ ] CSS styling intact

5. **API Validation**
   - [ ] Admin API starts
   - [ ] Swagger UI loads
   - [ ] Test endpoints via .http file
   - [ ] Responses valid

6. **Telemetry Validation**
   - [ ] OpenTelemetry traces visible in dashboard
   - [ ] Metrics being collected
   - [ ] Logging functioning

7. **Service Discovery Validation**
   - [ ] Services discover each other
   - [ ] Service-to-service communication works

---

## 9. Timeline and Effort Estimates

### 9.1 Per-Project Estimates

| Project | Complexity | Estimated Time | Dependencies | Risk Level |
|---------|------------|---------------|--------------|------------|
| SocialManager.Documentation | Low | 5 minutes | None | Low |
| SocialManager.Admin.API | Low | 15 minutes | None | Low |
| SocialManager.Admin | Medium | 30 minutes | None | Medium |
| SocialManager.Aspire.ServiceDefaults | Medium | 45 minutes | None | Medium |
| SocialManager | Medium | 30 minutes | Admin, Admin.API, ServiceDefaults | Medium |
| SocialManager.Aspire.Host | High | 60 minutes | SocialManager | High |

### 9.2 Phase Durations

**Phase 0: Preparation** - 10 minutes
- Verify .NET 10.0 SDK
- Clean working directory
- Commit pending changes

**Phase 1: Atomic Upgrade** - 30 minutes
- Update all 6 project files simultaneously
- Update all package references
- Restore dependencies

**Phase 2: Build and Fix** - 60 minutes
- Build solution
- Address any compilation errors
- Update code for breaking changes
- Rebuild and verify

**Phase 3: Testing and Validation** - 90 minutes
- Smoke test all components
- Manual testing of UI
- API testing
- Telemetry verification
- Service discovery testing

**Buffer**: 30 minutes for unexpected issues

**Total Estimated Time**: 3.5 - 4 hours

### 9.3 Resource Requirements

**Developer Skills Needed**:
- .NET/C# development experience
- Aspire orchestration knowledge
- Blazor WebAssembly experience
- ASP.NET Core API development

**Parallel Work Capacity**:
- Single developer can complete (Big Bang approach)
- Projects updated simultaneously, tested sequentially

---

## 10. Source Control Strategy

### 10.1 Branching Strategy

**Main upgrade branch**: `main` (as requested by user)
- All upgrade work done directly on main branch
- No feature branches required for this upgrade

### 10.2 Commit Strategy

**Recommended approach**: Single commit for entire upgrade
- **Single atomic commit** after all changes complete and verified
- Commit message: `chore: upgrade solution to .NET 10.0`
- Body should include:
  - List of all projects upgraded
  - Key package versions updated
  - Any breaking changes addressed

**Alternative approach** (if issues encountered):
- Commit after successful build: `chore: update all projects to .NET 10.0 and packages`
- Commit after fixes: `fix: address .NET 10.0 breaking changes`
- Commit after validation: `chore: verify .NET 10.0 upgrade complete`

### 10.3 Commit Message Template

```
chore: upgrade solution to .NET 10.0

Projects upgraded:
- SocialManager.Aspire.Host (net8.0 → net10.0)
- SocialManager.Aspire.ServiceDefaults (net8.0 → net10.0)
- SocialManager (net9.0 → net10.0)
- SocialManager.Admin (net9.0 → net10.0)
- SocialManager.Admin.API (net9.0 → net10.0)
- SocialManager.Documentation (net9.0 → net10.0)

Key package updates:
- Aspire.Hosting.AppHost 8.2.2 → 13.0.0
- Microsoft.Extensions.ServiceDiscovery 8.2.2 → 10.0.0
- Microsoft.Extensions.Http.Resilience 8.10.0 → 10.0.0
- ASP.NET Core packages 9.0.10 → 10.0.0
- OpenTelemetry packages 1.9.0 → 1.14.0-rc.1

Breaking changes addressed:
- [List any code changes made to address breaking changes]

All tests passing, application verified functional.
```

### 10.4 Review and Merge Process

**Review Requirements**:
- Self-review of all changes
- Verification that solution builds
- Verification that application runs
- Manual testing of key features

**Merge Criteria**:
- No merge needed (working directly on main)
- Final commit should be made only after complete validation

**Integration Validation**:
- CI/CD pipeline should run after commit
- Monitor for any deployment issues
- Verify production readiness

---

## 11. Success Criteria

### 11.1 Strategy-Specific Success Criteria

**Big Bang Strategy Success**: 
- All 6 projects upgraded in single operation
- No intermediate states or partial upgrades
- Entire solution builds and runs after upgrade
- All services orchestrated correctly by Aspire

### 11.2 Technical Success Criteria

- [ ] All 6 projects target net10.0
- [ ] All packages updated to specified versions:
  - [ ] Aspire.Hosting.AppHost 13.0.0
  - [ ] Microsoft.Extensions.ServiceDiscovery 10.0.0
  - [ ] Microsoft.Extensions.Http.Resilience 10.0.0
  - [ ] All ASP.NET Core packages 10.0.0
  - [ ] OpenTelemetry packages 1.14.0-rc.1
- [ ] Zero security vulnerabilities in dependencies
- [ ] Solution builds without errors
- [ ] Solution builds without warnings
- [ ] Aspire Host runs successfully
- [ ] Aspire Dashboard accessible
- [ ] All services start and function correctly

### 11.3 Quality Criteria

- [ ] Code quality maintained (no hacky workarounds)
- [ ] No deprecated APIs in use (except where unavoidable)
- [ ] Documentation updated (if applicable)
- [ ] No known functional regressions
- [ ] Performance acceptable (no degradation)

### 11.4 Process Criteria

- [ ] Big Bang strategy principles followed (atomic upgrade)
- [ ] All changes committed with clear commit message
- [ ] Upgrade completed within estimated timeline
- [ ] All validation steps completed successfully

---

## 12. Post-Upgrade Tasks

### 12.1 Immediate Tasks
- [ ] Monitor application in development for any issues
- [ ] Update any documentation referencing .NET versions
- [ ] Notify team of upgrade completion
- [ ] Update CI/CD pipeline if needed for .NET 10.0

### 12.2 Follow-up Tasks
- [ ] Monitor for stable releases of OpenTelemetry 1.14.x (currently RC)
- [ ] Consider upgrading to stable version when available
- [ ] Review Aspire 13.0 new features and capabilities
- [ ] Consider adopting new .NET 10.0 features where beneficial

---

## Appendix A: Quick Reference

### Command Quick Reference

```bash
# Verify .NET 10.0 SDK
dotnet --list-sdks

# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Build solution
dotnet build --no-incremental

# Run Aspire Host
dotnet run --project src/Aspire/SocialManager.Aspire.Host/SocialManager.Aspire.Host.csproj
```

### File Paths Quick Reference

- Solution: `SocialManager.slnx`
- Aspire Host: `src/Aspire/SocialManager.Aspire.Host/SocialManager.Aspire.Host.csproj`
- ServiceDefaults: `src/Aspire/SocialManager.Aspire.ServiceDefaults/SocialManager.Aspire.ServiceDefaults.csproj`
- Main App: `src/SocialManager/SocialManager.csproj`
- Admin: `src/Admin/SocialManager.Admin/SocialManager.Admin.csproj`
- Admin API: `src/Admin/SocialManager.Admin.API/SocialManager.Admin.API.csproj`
- Documentation: `documentation/SocialManager.Documentation/SocialManager.Documentation.csproj`

### Package Version Quick Reference

| Package | Old → New |
|---------|-----------|
| Aspire.Hosting.AppHost | 8.2.2 → 13.0.0 |
| Microsoft.Extensions.ServiceDiscovery | 8.2.2 → 10.0.0 |
| Microsoft.Extensions.Http.Resilience | 8.10.0 → 10.0.0 |
| Microsoft.AspNetCore.* | 9.0.10 → 10.0.0 |
| OpenTelemetry.Instrumentation.* | 1.9.0 → 1.14.0-rc.1 |

---

**End of Plan**