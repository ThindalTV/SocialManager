# Mobile App (MAUI)

## Overview

The SocialManager mobile application is built using **.NET MAUI with Blazor Hybrid**, providing a familiar web-based development experience while delivering native mobile performance across iOS and Android platforms. The app leverages the same **REST API** infrastructure as the web UI, ensuring feature parity and consistent data access while optimizing for mobile user experience through touch-first interactions and native platform capabilities.

**Blazor Hybrid** allows the app to reuse Blazor components from the web UI while seamlessly accessing native device features like camera, GPS location, biometric authentication, and platform-specific services through .NET MAUI's native bindings.

## Architecture Philosophy

### Blazor Hybrid Approach

The mobile app uses **Blazor Hybrid** architecture, which renders Blazor components inside a native WebView control. This approach provides several key advantages:

- **Component Reusability** - Direct reuse of Blazor components from the web UI
- **Familiar Development Model** - HTML/CSS/Razor syntax for UI development
- **Native Feature Access** - Full access to device features via .NET MAUI APIs
- **Shared Business Logic** - Common services and models across web and mobile
- **Unified Codebase** - Single technology stack for all platforms

### Technology Stack

**Core Framework:**
- .NET 10.0 with MAUI for cross-platform mobile development
- Blazor Hybrid for UI rendering in native WebView
- C# with nullable reference types for type safety

**UI Layer:**
- Blazor components with Razor syntax
- Shared CSS frameworks with web application
- JavaScript interop for advanced interactions
- Platform-specific adaptations where needed

**Native Integration:**
- MAUI Essentials for cross-platform device features
- Platform-specific services for iOS/Android capabilities
- JavaScript interop bridges for web-to-native communication

**Data & State:**
- SQLite for local data persistence
- Entity Framework Core for database access
- Blazor state management patterns
- HTTP client for API communication

**Media & Sensors:**
- Native camera and photo library access
- GPS geolocation services
- Biometric authentication (Face ID, Touch ID, Fingerprint)
- Accelerometer, gyroscope, and other sensors
- Haptic feedback and vibration

## Core Features

### 1. Blog Post Management

The mobile app provides comprehensive blog authoring capabilities optimized for mobile devices:

**Content Creation:**
- Rich text editing with formatting options (bold, italic, headings, lists, links)
- Markdown support with live preview
- Inline media embedding using native camera or gallery
- Automatic draft saving to prevent data loss
- Offline drafting with sync when connection restored
- Tag management with autocomplete suggestions
- Category assignment for organization
- Featured image selection from device
- SEO metadata fields (title, description, meta tags)
- Optional GPS location tagging for posts

**Publishing Workflow:**
- Immediate publishing to blog via API
- Schedule posts for future publication with date/time picker
- Time zone selection for global audience targeting
- Preview mode before publishing
- Revision history with version tracking
- Publication status indicators (Draft, Scheduled, Published, Archived)
- Cross-platform social sharing during publish
- Optional biometric confirmation before publishing

**Content Management:**
- Scrollable list of all posts with virtualization for performance
- Filter and search capabilities
- Full post detail view with edit option
- Quick edit for minor changes
- Version control timeline
- Analytics preview with engagement metrics
- Delete/archive with confirmation
- Republish to update timestamps
- Pull-to-refresh to fetch latest data

### 2. Social Post Management

Create and manage social media posts across multiple platforms:

**Composition:**
- Platform selection with checkboxes (X, Instagram, Facebook, LinkedIn, BlueSky, Mastodon, Reddit)
- Real-time character counter per platform
- Visual indicators for platform-specific character limits
- AI-powered hashtag suggestions
- @ mention autocomplete from connected accounts
- Native camera/gallery integration for media
- Multi-image carousel creation with reordering
- Automatic link preview generation
- GPS location tagging via native services
- Post scheduling with calendar picker
- Voice-to-text dictation (future)

**Multi-Platform Publishing:**
- Unified composer for all platforms
- Platform-specific preview tabs
- Individual customization per platform
- Simultaneous publishing to all selected platforms
- Staggered scheduling with different times per platform
- Progress indicators for each platform
- Retry mechanism for failed posts
- Draft synchronization across devices

**Post Management:**
- Card-based feed view with infinite scroll
- Filter by specific platforms
- Full-text search across all posts
- Inline performance metrics (likes, shares, comments)
- Swipe-to-delete gestures
- Repost/reshare functionality
- Detailed analytics modal
- Pagination for historical posts

### 3. Media Management

Comprehensive media handling optimized for mobile:

**Photo Features:**
- Native camera capture
- Photo library access with multi-select
- Image cropping and rotation
- Filter application
- Automatic compression before upload
- Accessibility alt text input
- Previously uploaded media library
- Thumbnail generation
- Drag-and-drop reordering

**Video Features:**
- Native video recording
- Video gallery selection
- Basic trimming and clipping
- Thumbnail frame selection
- Automatic compression
- Platform-specific limit enforcement
- Background upload continuation
- Progress tracking with pause/resume
- HTML5 video preview

**Library Management:**
- Grid view of all uploaded media
- Tag-based organization
- Search and filtering
- Asset reuse across posts
- Batch operations (delete, tag)
- Storage usage analytics
- Sync status with web UI

### 4. Cross-Platform Content Distribution

Unified workflow for distributing content across multiple channels:

**Blog-to-Social Automation:**
- Toggle to auto-generate social posts from blog excerpts
- Configurable templates for content transformation
- Platform-specific conversion rules
- Manual editing before publishing
- Automatic link-back to blog with UTM tracking
- Hashtag extraction from blog tags
- Featured image reuse in social posts
- Time-delayed social posting after blog publish

**Content Synchronization:**
- Single source of truth for multi-platform content
- Sync status badges per platform
- Divergence warnings when content differs
- Bulk sync to update all platforms
- Selective sync with platform checkboxes
- Conflict resolution with diff view

### 5. Stream Controls (TBD)

Mobile monitoring and control for live streams:

**Monitoring:**
- Live status cards for active streams
- Real-time viewer count and engagement
- Multi-platform streaming status
- Stream health indicators (bitrate, dropped frames)
- Connection quality metrics

**Basic Controls:**
- Start/stop stream via API commands (not local capture)
- Real-time chat feed using SignalR
- Push notifications for stream events (raids, follows, subs)
- Viewer analytics dashboard

**Note:** Full streaming with video capture remains desktop-only due to hardware requirements. Mobile focuses on monitoring and API-based control.

### 6. Mobile-Specific Features

Native mobile capabilities that enhance the experience:

**Device Integration:**
- Share extension to receive content from other apps
- Biometric authentication (Face ID, Touch ID, Fingerprint)
- Haptic feedback for important actions
- Adaptive app icons (Android)
- Home screen quick actions (3D Touch/long-press)
- Home screen widgets displaying stats (future)
- Voice assistant shortcuts (Siri/Google Assistant) (future)

**Mobile Optimizations:**
- Touch-optimized gesture controls (swipe, pinch, pull-to-refresh)
- Bottom sheet UI patterns for mobile actions
- Floating action button for quick post creation
- Responsive image loading based on screen size
- Automatic dark mode support
- Reduced motion respect for accessibility
- Offline action queue with background sync

## Platform-Specific Features

### iOS Capabilities

**Requirements:**
- iOS 15.0 or later
- Universal app for iPhone and iPad
- Xcode 15.0+ for development

**Native Features:**
- Face ID and Touch ID biometric authentication
- Haptic feedback with system haptics
- 3D Touch/Haptic Touch quick actions
- iOS share extension integration
- Universal links for deep linking
- SwiftUI widgets (future)
- Siri Shortcuts (future)
- iCloud draft synchronization (future)

**Permissions:**
- Camera access for photo/video capture
- Photo library access for media selection
- Microphone access for video recording
- Location access for GPS tagging
- Speech recognition for voice input

### Android Capabilities

**Requirements:**
- Android 8.0 (API 26) or later
- Target SDK: Android 15 (API 35)
- Responsive layouts for phones and tablets

**Native Features:**
- Material Design 3 with dynamic theming
- Adaptive icons with system theme integration
- Android share intent integration
- Notification channels for categorization
- Fingerprint and face unlock biometrics
- Jetpack Glance widgets (future)
- Dynamic shortcuts from app icon
- Picture-in-picture mode (future)

**Permissions:**
- Internet access for API communication
- Camera and media permissions
- Location access (fine and coarse)
- Foreground service for uploads
- Push notification permissions
- Audio recording for video
- Vibration for haptic feedback

### Cross-Platform Design Patterns

**Navigation:**
- iOS: Bottom tab bar with left-edge swipe for back
- Android: Bottom navigation with system back gesture
- Conditional rendering based on platform detection

**UI Patterns:**
- iOS: System alert style and action sheets
- Android: Material Design dialogs and bottom sheets
- Platform-specific styling via CSS classes

**Touch Interactions:**
- iOS: Edge swipe back, long-press context menu
- Android: Back button/gesture, long-press menu
- Custom gesture handlers via JavaScript interop

**Typography & Layout:**
- iOS: San Francisco font, 44pt touch targets
- Android: Roboto font, 48dp touch targets
- Responsive spacing for different screen sizes

## Authentication & Security

### OAuth 2.0 with PKCE

The mobile app implements secure authentication using OAuth 2.0 Authorization Code flow with PKCE (Proof Key for Code Exchange):

**Authentication Flow:**
1. Generate cryptographic code verifier and challenge
2. Open system browser for OAuth authorization
3. User authenticates and grants permissions
4. Receive authorization code via deep link
5. Exchange code + verifier for access token
6. Store token securely in platform keychain
7. Use token for authenticated API requests

**Token Management:**
- Automatic token refresh when expired
- Retry logic for failed requests after refresh
- Secure storage in iOS Keychain or Android EncryptedSharedPreferences
- Force re-authentication if refresh fails

### API Security

**Communication Security:**
- HTTPS-only connections
- TLS/SSL certificate validation
- Bearer token authentication
- Client-side rate limiting
- Request timeouts to prevent hangs

**Additional Security:**
- Optional biometric app lock
- Automatic session timeout
- Secure credential storage
- No logging of sensitive data

## Offline Support & Caching

### Local Data Storage

**SQLite Database:**
- Draft posts stored locally
- Recent published posts cached
- Media references and metadata
- User profile information
- App settings and preferences

**Cache Strategy:**
- Blog posts cached for 15 minutes
- Social posts cached for 5 minutes
- User profile cached for 1 hour
- Media thumbnails cached indefinitely
- Analytics data cached for 30 minutes

### Offline Capabilities

**Work Offline:**
- View previously loaded content
- Create and edit drafts
- Queue publish operations
- Edit local drafts without connection
- Visual offline indicators

**Sync When Online:**
- Automatic upload prompt when reconnected
- Conflict resolution for changed content
- Sequential processing of queued operations
- Background sync status indicators

## Performance Optimization

### Mobile-Specific Optimizations

**Image Handling:**
- Lazy loading as images scroll into view
- Low-resolution placeholder while loading
- Automatic compression (80% JPEG quality)
- Platform-appropriate formats (WebP/HEIC)
- Maximum resolution limits (2048x2048)
- Progressive image loading

**List Performance:**
- Virtual scrolling for long lists
- Only render visible items
- Incremental loading on scroll
- Template reuse for consistency
- Batch UI updates

**Network Optimization:**
- Request batching where possible
- GZIP compression for responses
- Conditional requests with ETags
- Background media downloads
- Priority loading for critical data

**Memory Management:**
- LRU cache for decoded images
- Proper resource disposal
- Weak references for large objects
- Memory usage monitoring

## User Experience Design

### Design Principles

**Mobile-First:**
- Touch-optimized controls (44pt/48dp minimum)
- Thumb-friendly positioning
- Clear visual hierarchy
- Consistent interaction patterns
- Immediate feedback for actions
- Graceful error handling

**Accessibility:**
- Screen reader support with proper labels
- Dynamic type for user font preferences
- High contrast color ratios
- Hardware keyboard navigation
- Voice assistant compatibility
- Reduced motion respect

**Visual Design:**
- Automatic dark mode support
- Manual theme override option
- Consistent theming across screens
- Optimized color palettes for both modes

**Localization (Future):**
- Multi-language support
- Locale-appropriate date/time formatting
- Number format localization
- Right-to-left language support

## Push Notifications (Future)

### Notification Types

**Content Updates:**
- Scheduled post published confirmation
- Post engagement milestones
- Content publishing reminders

**Platform Events:**
- Social platform mentions and replies
- Stream events (raids, follows, subscriptions)
- High-engagement alerts

**System Notifications:**
- App updates and maintenance
- Important announcements

### Platform Features

**Android Notification Channels:**
- Separate channels for different types
- User-configurable importance
- Custom sounds and vibrations

**Notification Actions:**
- Quick reply from notification
- Deep link to specific content
- Dismiss or snooze options

## Testing Strategy

### Testing Approach

**Unit Testing:**
- Business logic in services
- API communication with mocked responses
- Value converters and helpers
- Utility functions

**Integration Testing:**
- Real API calls in staging environment
- Database CRUD operations
- OAuth authentication flow
- Media upload end-to-end

**UI Testing:**
- Navigation flows
- Form validation and submission
- Touch gesture interactions
- Visual regression testing

**Platform Testing:**
- Physical device testing (iOS and Android)
- Various screen sizes and OS versions
- Accessibility with screen readers
- Performance profiling

## Deployment & Distribution

### iOS Distribution

**App Store Release:**
- App Store Connect submission
- App Store review compliance
- Manual or automatic release
- Version management

**TestFlight Beta:**
- Internal testing (100 testers)
- External testing (10,000 testers)
- Feedback collection
- Crash report analysis

### Android Distribution

**Google Play Release:**
- Play Console submission
- Progressive rollout support
- Release tracks (internal, closed, open, production)
- Play Store review

**Beta Testing:**
- Internal track for team testing
- Closed track for invite-only beta
- Open track for public beta

### CI/CD Pipeline

**Automated Processes:**
- Build on every commit
- Automated version bumping
- Code signing with stored secrets
- Artifact storage

**Quality Gates:**
- Unit test execution on PRs
- UI tests on staging builds
- Code coverage enforcement
- Static code analysis

**Deployment Automation:**
- Auto-deploy to beta tracks
- Manual approval for production
- Quick rollback capability
- Release notes from commits

## Development Roadmap

### Phase 1: MVP (Current Focus)
- ? Project architecture and setup
- ? OAuth authentication
- ? Blog post creation and editing
- ? Social post creation and publishing
- ? Native media upload
- ? Basic offline support
- ? iOS and Android platform support

### Phase 2: Enhanced Features
- ? Multi-platform publishing UI
- ? Post scheduling calendar
- ? Draft auto-save and recovery
- ? Rich text editor enhancements
- ? Image editing tools
- ? Video trimming
- ? Analytics dashboard

### Phase 3: Advanced Capabilities
- ?? Push notifications
- ?? Home screen widgets
- ?? Share extensions
- ?? Voice assistant shortcuts
- ?? Advanced analytics
- ?? AI content recommendations
- ?? Team collaboration

### Phase 4: Streaming Integration (TBD)
- ?? Stream monitoring dashboard
- ?? Real-time chat viewing
- ?? Basic stream controls via API
- ?? Stream event notifications
- ?? Viewer analytics

**Legend:**
- ? Completed
- ? In Progress
- ?? Planned

## Best Practices

### Development Guidelines

**Code Quality:**
- Follow C# naming conventions
- Use nullable reference types
- Async/await for all async operations
- Constructor dependency injection
- Strict MVVM separation
- Code reviews before merge

**Performance:**
- Profile before optimizing
- Defer expensive operations
- Dispose resources properly
- Offload work to background threads
- Focus on maintainability first

**Security:**
- Never log credentials or tokens
- Validate and sanitize user input
- HTTPS-only communication
- Secure storage for sensitive data
- Keep dependencies updated

**User Experience:**
- Immediate feedback for actions
- Progress indicators for long operations
- Clear, actionable error messages
- Graceful offline degradation
- Accessibility from the start

## Summary

The SocialManager mobile app delivers a **native, high-performance experience** for content creators on iOS and Android by leveraging **Blazor Hybrid** architecture. Key advantages include:

**Architecture Benefits:**
- Component reuse with web UI for faster development
- Native device feature access via MAUI Essentials
- Familiar web development patterns (HTML/CSS/C#)
- Single codebase for multiple platforms

**Native Capabilities:**
- Camera and photo library integration
- GPS location services
- Biometric authentication
- Push notifications (future)
- Platform-specific optimizations

**Core Functionality:**
- Full blog post authoring and publishing
- Multi-platform social posting
- Rich media management
- Cross-platform content distribution
- Stream monitoring and control (TBD)

**Mobile Optimizations:**
- Touch-first design patterns
- Offline drafting and queueing
- Background synchronization
- Performance-optimized rendering
- Platform-specific UI adaptations

The app uses the **same REST API** as the web UI, ensuring consistency while providing mobile-specific enhancements through native feature integration. Future phases will expand capabilities with push notifications, widgets, and deeper analytics integration.
