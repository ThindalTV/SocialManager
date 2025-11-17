# Restreamer Application Intent

## Overview
The Restreamer is a client-side application that enables simultaneous streaming to multiple platforms (Twitch, YouTube, Facebook Gaming, etc.) from a single source stream. While hosted locally on the user's machine, it maintains a persistent connection to SocialManager for remote configuration, monitoring, and command & control (C&C) functionality.

## Core Objectives

### Primary Goals
1. **Multi-Platform Streaming**: Accept a single RTMP/SRT input stream and simultaneously restream to multiple configured destinations
2. **Remote Management**: Full control and configuration through SocialManager web interface
3. **Real-Time Monitoring**: Stream health metrics, bandwidth usage, and connection status visible in SocialManager
4. **Flexible Deployment**: Lightweight client application that runs on user's hardware (Windows, Linux, macOS)

### Secondary Goals
- Minimize latency and resource overhead
- Provide fallback and reconnection logic for stable streaming
- Support stream transcoding options for platform-specific requirements
- Enable stream recording alongside live streaming

## Architecture

### Client Application (Restreamer)
**Technology Stack**:
- **.NET 10 Console Application** or **Windows Service/Linux Daemon**
- **FFmpeg** integration for stream processing and transcoding
- **SignalR Client** for persistent C&C connection to SocialManager
- **RTMP/SRT Server** for accepting incoming streams

**Key Components**:
1. **Stream Ingestion Layer**
   - RTMP/SRT server listening for incoming streams
   - Support for common streaming software (OBS, Streamlabs, XSplit)
   - Stream key validation against SocialManager

2. **Stream Processing Engine**
   - FFmpeg-based transcoding pipeline
   - Multi-output stream management
   - Adaptive bitrate handling per platform

3. **Distribution Manager**
   - Parallel RTMP/SRT output to multiple destinations
   - Per-platform configuration (resolution, bitrate, codec)
   - Connection health monitoring and auto-reconnection

4. **C&C Client**
   - SignalR persistent connection to SocialManager API
   - Real-time command execution (start/stop streams, update configs)
   - Status reporting (stream health, resource usage, errors)

5. **Registration & Authentication**
   - Initial registration with SocialManager instance
   - Secure token-based authentication
   - Device identity and capability reporting

### SocialManager Integration

**New API Endpoints**:
- `/api/restreamer/register` - Register new restreamer client
- `/api/restreamer/devices` - List registered restreamer devices
- `/api/restreamer/devices/{id}/configuration` - Get/update device configuration
- `/api/restreamer/devices/{id}/status` - Real-time device status
- `/api/restreamer/devices/{id}/commands` - Send C&C commands

**SignalR Hub** (`RestreamerHub`):
- `RegisterDevice(DeviceInfo)` - Initial device registration
- `ReportStatus(StatusUpdate)` - Periodic status updates from client
- `UpdateConfiguration(Configuration)` - Push configuration changes
- `ExecuteCommand(Command)` - Remote command execution
- `NotifyEvent(StreamEvent)` - Stream lifecycle events

**UI Components**:
1. **Restreamer Management Dashboard**
   - List of registered restreamer devices
   - Real-time status indicators
   - Quick actions (start/stop, restart)

2. **Device Configuration Page**
   - Stream destinations configuration
   - Transcoding settings per platform
   - Quality and performance presets

3. **Monitoring View**
   - Live stream health metrics
   - Bandwidth usage graphs
   - Error logs and diagnostics

## User Workflow

### Initial Setup
1. User downloads and installs Restreamer client application
2. Client prompts for SocialManager instance URL
3. User authenticates through browser (OAuth flow)
4. Client registers with SocialManager and receives configuration
5. Client displays local RTMP endpoint for streaming software

### Daily Operation
1. User starts stream in OBS/Streamlabs pointing to local Restreamer
2. User opens SocialManager web interface
3. User selects target platforms from configured social accounts
4. User clicks "Start Restream" in SocialManager
5. Restreamer begins distributing to all selected platforms
6. User monitors stream health in real-time dashboard
7. User can add/remove platforms during active stream
8. User ends stream; Restreamer stops all outputs

### Remote Control Scenarios
- **Emergency Stop**: Immediately terminate all streams from web interface
- **Platform Toggle**: Add or remove streaming destinations mid-stream
- **Quality Adjustment**: Change transcoding settings on-the-fly
- **Recording Control**: Start/stop local recording alongside streaming

## Technical Requirements

### Restreamer Client
**System Requirements**:
- CPU: Multi-core processor (4+ cores recommended for multi-platform streaming)
- RAM: 4GB minimum, 8GB+ recommended
- Network: Upload bandwidth sufficient for all target platforms combined
- OS: Windows 10+, Ubuntu 20.04+, macOS 12+

**Dependencies**:
- FFmpeg 6.0+ (bundled or system-installed)
- .NET 10 Runtime
- Network connectivity to SocialManager instance

### SocialManager Backend
**New Services**:
- `RestreamerManagementService` - Device lifecycle and configuration management
- `RestreamerCommandService` - Command queuing and execution tracking
- `RestreamerMonitoringService` - Health checks and alerting

**Database Schema**:
```csharp
// RestreamerDevice entity
public class RestreamerDevice
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public string DeviceName { get; set; }
    public string DeviceIdentifier { get; set; } // Hardware/machine ID
    public string Version { get; set; } // Client version
    public RestreamerCapabilities Capabilities { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime LastSeenAt { get; set; }
    public RestreamerStatus Status { get; set; }
    public RestreamerConfiguration Configuration { get; set; }
}

// RestreamerConfiguration
public class RestreamerConfiguration
{
    public string IngestProtocol { get; set; } // RTMP, SRT
    public int IngestPort { get; set; }
    public List<StreamDestination> Destinations { get; set; }
    public TranscodingSettings Transcoding { get; set; }
    public RecordingSettings Recording { get; set; }
}

// StreamDestination
public class StreamDestination
{
    public Guid Id { get; set; }
    public string Platform { get; set; } // Twitch, YouTube, etc.
    public Guid SocialAccountId { get; set; } // Link to configured account
    public string RtmpUrl { get; set; }
    public string StreamKey { get; set; }
    public bool Enabled { get; set; }
    public DestinationSettings Settings { get; set; }
}
```

### Security Considerations
1. **Authentication**:
   - Client uses OAuth 2.0 device flow for initial registration
   - JWT tokens for API authentication
   - Automatic token refresh

2. **Authorization**:
   - Restreamer devices scoped to user and tenant
   - Role-based access for multi-user teams
   - Stream keys encrypted at rest

3. **Communication**:
   - TLS/SSL for all SocialManager connections
   - SignalR over WSS (WebSocket Secure)
   - Certificate pinning for production deployments

4. **Data Protection**:
   - Stream keys never logged or displayed in UI
   - Secure credential storage on client (Windows Credential Manager, macOS Keychain, Linux Secret Service)
   - Configuration encryption in transit

## Features & Capabilities

### Phase 1: MVP (Minimum Viable Product)
- ? Client application with RTMP ingestion
- ? FFmpeg integration for stream passthrough
- ? Multi-platform RTMP output
- ? SignalR-based C&C connection
- ? Basic SocialManager UI for device management
- ? Start/stop streaming commands
- ? Real-time status reporting

### Phase 2: Enhanced Control
- ? Per-platform transcoding profiles
- ? Adaptive bitrate configuration
- ? Advanced monitoring dashboard with graphs
- ? Stream health alerts and notifications
- ? Multiple simultaneous streams per device
- ? Stream preview/thumbnails in SocialManager

### Phase 3: Advanced Features
- ? Local recording with cloud upload
- ? Stream overlay injection
- ? Audio mixing and multiple audio tracks
- ? Instant replay buffer
- ? Multi-device load balancing
- ? Stream scheduling and automation

### Phase 4: Professional Tools
- ? Custom RTMP authentication plugins
- ? WebRTC ingestion option
- ? NDI input support
- ? Hardware encoding (NVENC, QuickSync, VCE)
- ? Backup stream failover
- ? CDN integration for private restreaming

## Integration Points

### With Existing SocialManager Features

**Social Providers**:
- Restreamer destinations automatically populated from connected social accounts
- OAuth tokens used to retrieve platform-specific RTMP endpoints
- Account status (live/offline) synchronized with restreamer state

**Streaming Module**:
- Shared stream metadata (title, description, category)
- Coordinated "Go Live" across platforms
- Unified chat integration during restream

**Analytics**:
- Combined viewer statistics across platforms
- Stream performance metrics (dropped frames, bitrate)
- Historical restream session data

**Webhooks**:
- Stream start/stop events
- Device online/offline notifications
- Alert conditions (connection lost, low bandwidth)

## Development Considerations

### Client Application Structure
```
SocialManager.Restreamer.Client/
??? Program.cs                          # Entry point, host configuration
??? Services/
?   ??? RegistrationService.cs          # Device registration logic
?   ??? CommandControlService.cs        # SignalR C&C client
?   ??? StreamIngestionService.cs       # RTMP/SRT server
?   ??? StreamProcessingService.cs      # FFmpeg orchestration
?   ??? DistributionService.cs          # Multi-platform output
?   ??? MonitoringService.cs            # Status reporting
??? Models/
?   ??? DeviceInfo.cs
?   ??? StreamConfiguration.cs
?   ??? StatusUpdate.cs
??? FFmpeg/
?   ??? FFmpegWrapper.cs                # Process management
?   ??? TranscodingPipeline.cs          # Encoding configuration
?   ??? StreamMonitor.cs                # Health checking
??? Configuration/
    ??? appsettings.json
    ??? RestreamerSettings.cs
```

### Backend API Structure
```
SocialManager.API/Features/Restreamer/
??? Controllers/
?   ??? RestreamerController.cs
??? Hubs/
?   ??? RestreamerHub.cs
??? Services/
?   ??? RestreamerManagementService.cs
?   ??? RestreamerCommandService.cs
?   ??? RestreamerMonitoringService.cs
??? Models/
?   ??? RestreamerDevice.cs
?   ??? RestreamerConfiguration.cs
?   ??? StreamDestination.cs
?   ??? Commands/
?       ??? StartStreamCommand.cs
?       ??? StopStreamCommand.cs
?       ??? UpdateConfigurationCommand.cs
??? Events/
    ??? DeviceRegisteredEvent.cs
    ??? StreamStartedEvent.cs
    ??? StreamStoppedEvent.cs
```

### Frontend Components
```
SocialManager.Admin/Pages/Restreamer/
??? Index.razor                         # Main restreamer dashboard
??? DeviceDetails.razor                 # Individual device management
??? Configuration.razor                 # Settings and destinations
??? Monitoring.razor                    # Real-time stream health

SocialManager.Admin/Components/Restreamer/
??? DeviceCard.razor                    # Device status card
??? StreamDestinationList.razor         # Platform selection
??? StatusIndicator.razor               # Connection/stream status
??? MetricsGraph.razor                  # Performance charts
```

## Testing Strategy

### Client Testing
- **Unit Tests**: FFmpeg command generation, configuration parsing
- **Integration Tests**: SignalR connection, command execution
- **End-to-End Tests**: Full stream pipeline with test RTMP server

### Backend Testing
- **Unit Tests**: Command handling, configuration validation
- **Integration Tests**: SignalR hub, database operations
- **Load Tests**: Multiple concurrent restreamer connections

### System Testing
- **Stream Quality Tests**: Verify output matches input quality
- **Failover Tests**: Connection loss and recovery
- **Performance Tests**: CPU/memory usage under load

## Deployment & Distribution

### Client Packaging
- **Windows**: MSI installer with .NET runtime bundled
- **macOS**: DMG package with notarization
- **Linux**: .deb and .rpm packages, snap/flatpak
- **Docker**: Container image for headless deployments

### Auto-Update Mechanism
- Client checks SocialManager for version updates
- Background download of new versions
- User-prompted installation (or automatic for minor updates)

### Documentation
- Installation guides per platform
- OBS/Streamlabs configuration tutorials
- Troubleshooting common issues
- Network/firewall setup requirements

## Success Metrics

### Performance KPIs
- Stream startup latency < 5 seconds
- CPU usage < 50% for 3 simultaneous 1080p60 streams
- < 0.5% dropped frames under normal conditions
- Client-to-SocialManager latency < 200ms

### Reliability Metrics
- 99.9% uptime for registered devices (excluding user network issues)
- < 5 second reconnection time after network interruption
- Automatic recovery for 95% of transient failures

### User Experience
- < 10 minute setup time for new users
- One-click restream activation
- Real-time status updates (< 2 second delay)

## Future Enhancements

### Advanced Scenarios
- **Cloud Restreamer**: SocialManager-hosted restreaming service (no client installation)
- **Mobile Restreamer**: iOS/Android apps for mobile streaming
- **Browser-Based Ingestion**: WebRTC capture directly from browser
- **Scene Switching**: Control OBS scenes remotely through SocialManager
- **AI Features**: Auto-highlight generation, real-time moderation, content warnings

### Enterprise Features
- Multi-user access control for team environments
- Centralized billing and usage tracking
- SLA guarantees and priority support
- Custom branding and white-label options

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| FFmpeg dependency complexity | High | Bundle tested FFmpeg builds, fallback to system version |
| Network bandwidth limitations | High | Bandwidth calculator in UI, quality presets, warnings |
| Platform RTMP endpoint changes | Medium | Regular endpoint validation, user notifications |
| Client crashes during stream | High | Watchdog process, automatic restart, state persistence |
| Security vulnerabilities in stream keys | High | Encryption at rest/transit, credential rotation prompts |

## Conclusion

The Restreamer application represents a powerful addition to SocialManager's streaming capabilities, enabling creators to expand their reach across multiple platforms simultaneously. By maintaining remote control through SocialManager while running locally on user hardware, it offers the best of both worlds: centralized management with low-latency, high-performance streaming.

The phased approach allows for rapid MVP delivery while building toward a comprehensive professional-grade restreaming solution that can scale from individual creators to large organizations.
