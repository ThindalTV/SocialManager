# .NET 10.0 Big Bang Upgrade for SocialManager

## Overview

Apply the Big Bang migration described in the Plan: upgrade all 6 projects to `net10.0`, update package versions per Plan §5, fix breaking changes per Plan §6, and perform automated smoke tests (no manual UI tasks included). Tasks are sized to be executable by an LLM/script and include deterministic verification steps.

**Progress**: 3/4 tasks complete (75%) ![75%](https://progress-bar.xyz/75)

## Tasks

### [✓] TASK-001: Verify prerequisites and workspace state *(Completed: 2025-11-11 21:29)*
**References**: Plan §3.1, Plan §8.1

- [✓] (1) Verify .NET 10.0 SDK is installed: run `dotnet --list-sdks` and confirm `10.x` appears (Plan §8.1)  
- [✓] (2) Ensure working directory is clean (no unstaged or uncommitted changes) per Plan §3.1 (**Verify**)  
- [✓] (3) Commit or stash any pending local changes if required (Plan §3.1) — leave working tree in a clean state (**Verify**)

---

### [✓] TASK-002: Atomic framework and package upgrade across all projects *(Completed: 2025-11-11 21:32)*
**References**: Plan §2.1, Plan §3.2, Plan §4 (per-project steps), Plan §5 (Package Update Reference), Plan §6 (Breaking Changes Catalog)

- [✓] (1) Update `TargetFramework` to `net10.0` in all projects listed in Plan §3.2 / Plan §4:
      - `src/Aspire/SocialManager.Aspire.ServiceDefaults/SocialManager.Aspire.ServiceDefaults.csproj`
      - `src/Admin/SocialManager.Admin/SocialManager.Admin.csproj`
      - `src/Admin/SocialManager.Admin.API/SocialManager.Admin.API.csproj`
      - `src/SocialManager/SocialManager.csproj`
      - `src/Aspire/SocialManager.Aspire.Host/SocialManager.Aspire.Host.csproj`
      - `documentation/SocialManager.Documentation/SocialManager.Documentation.csproj`
- [✓] (2) Update NuGet package references per Plan §5 (batch update across projects):
      - Include critical updates: `Aspire.Hosting.AppHost` → `13.0.0`, `Microsoft.Extensions.ServiceDiscovery` → `10.0.0`, `Microsoft.Extensions.Http.Resilience` → `10.0.0`, ASP.NET Core packages → `10.0.0`, OpenTelemetry → `1.14.0-rc.1` where listed (Plan §5)  
- [✓] (3) Run `dotnet restore SocialManager.slnx` and confirm all packages restore successfully (**Verify**)
- [✓] (4) Build solution to surface compilation issues: `dotnet build SocialManager.slnx --no-incremental` (Plan §8.2)
- [✓] (5) Fix all compilation errors identified, referencing Plan §6 (Breaking Changes Catalog) for expected patterns:
      - Address Aspire host API changes in `Program.cs` and resource registration (Plan §6, Project: SocialManager.Aspire.Host)
      - Update service discovery registration patterns in `ServiceDefaults` (`Extensions.cs`) (Plan §6, Project: SocialManager.Aspire.ServiceDefaults)
      - Adjust Blazor component or hosting code per Plan §6 for `SocialManager.Admin` and `SocialManager` where needed
- [✓] (6) Rebuild solution after fixes and confirm solution builds with 0 errors (**Verify**)
- [✓] (7) Confirm no new package or runtime conflicts reported in restore/build output (**Verify**)

---

### [✓] TASK-003: Automated smoke tests and runtime start verification *(Completed: 2025-11-11 21:45)*
**References**: Plan §8.2, Plan §8.3 (Build & Runtime Validation)

- [✓] (1) Run clean + restore + build to validate final state:
      - `dotnet clean`
      - `dotnet restore SocialManager.slnx`
      - `dotnet build SocialManager.slnx --no-incremental` (**Verify**) — expect 0 errors
- [✓] (2) Start Aspire host and verify it launches without fatal errors:
      - `dotnet run --project src/Aspire/SocialManager.Aspire.Host/SocialManager.Aspire.Host.csproj` (Plan §8.2)
      - Confirm process exits with code 0 OR that the host reports "started" / listening in logs within a bounded timeout (**Verify**)
- [✓] (3) Verify basic runtime health checks (automatable checks only):
      - Check application process logs for absence of startup exceptions or fatal errors within first 60s (**Verify**)  
      - (No manual UI/dashboard checks included — dashboard accessibility and visual checks are excluded as non-automatable per strategy)
- [✓] (4) If any automated runtime verification fails, record failures and return to TASK-002 to fix compilation/runtime issues; ensure deterministic fixes are applied and re-run this task (**Verify**)

---

### [▶] TASK-004: Final commit of upgrade changes
**References**: Plan §10 (Source Control Strategy), Plan §11 (Success Criteria)

- [▶] (1) Create a single atomic commit with the message per Plan §10.3:
      - Commit message title: `chore: upgrade solution to .NET 10.0`
      - Commit body should list upgraded projects and key package updates (use Plan §10.3 template)  
- [ ] (2) Verify commit was created successfully and repository is in clean state (**Verify**)  
- [ ] (3) Confirm key success criteria from Plan §11 are met:
      - All 6 projects target `net10.0` (**Verify**)  
      - Critical packages updated: `Aspire.Hosting.AppHost` → `13.0.0`, `Microsoft.Extensions.ServiceDiscovery` → `10.0.0`, `Microsoft.Extensions.Http.Resilience` → `10.0.0` (**Verify**)  
      - Solution builds with 0 errors and no new restore/build conflicts (**Verify**)  

--- 

Notes:
- Manual UI, visual dashboard validation, and other human-judgment steps described in the Plan are intentionally excluded from these tasks (per strategy exclusions). Those remain post-upgrade manual checks for the team.
- Large lists (per-project file lists, package tables, breaking-change details) are referenced to the Plan (§4, §5, §6) — see those sections for exact file/package versions and per-project notes.