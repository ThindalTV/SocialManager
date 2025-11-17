# Socials

## Intent Overview
The `Socials` intent is designed to facilitate interactions related to social media platforms and online communities. This intent can be used to manage social media accounts, post updates, retrieve information, and engage with followers.

Using a single interface, users can perform various actions across multiple social media platforms, such as posting content, fetching user profiles, and managing interactions.

On top of this, from the same interface, they can also compose and publish [blog posts](Blog.md) and other content.

## Social Platforms Supported
- Facebook (TODO)
- X (TODO)
- Instagram (TODO)
- LinkedIn (TODO)
- BlueSky (TODO)
- Mastodon (TODO)
- Reddit (TODO)

## Unified Content Model

The SocialManager uses a unified content model that enables seamless content creation and distribution across both blog posts and social media platforms.

### Content Structure
All content shares a common structure:
- **Text Content** - Main message body with platform-specific formatting
- **Media Attachments** - Images, videos, GIFs, and documents
- **Metadata** - Tags, categories, visibility settings, and platform targeting
- **Status** - Draft, scheduled, published, or archived states
- **Cross-posting Configuration** - Specification of which platforms to publish to

### Content States
- **Draft** - Work in progress, not published anywhere
- **Scheduled** - Queued for future publication with specific date/time per platform
- **Published** - Live on one or more platforms
- **Archived** - Historical content retained for analytics but no longer actively promoted

## Cross-Platform Publishing

### Publishing Workflow
1. **Compose Content** - Create content once in the unified interface
2. **Select Target Platforms** - Choose which social platforms and/or blog to publish to
3. **Platform Adaptation** - Automatically or manually adjust content for each platform's requirements
4. **Review & Publish** - Preview platform-specific versions before publishing
5. **Track & Engage** - Monitor performance and respond to engagement across all platforms

### Auto-Generation from Blog Posts
When publishing a blog post, the system can automatically generate social media posts:
- **Summary Extraction** - Pull key points or first paragraph as social content
- **Character Limit Handling** - Truncate and append "Read more" links for platforms like X
- **Hashtag Suggestions** - Recommend relevant hashtags based on blog tags and categories
- **Media Selection** - Use featured image or extract inline images for social previews

### Platform-Specific Adaptations
- **Character Limits** - X (280 chars), LinkedIn (3000 chars), etc.
- **Media Format Requirements** - Aspect ratios, file sizes, video length limits
- **Link Handling** - URL shortening, preview cards, link placement strategies
- **Hashtag Conventions** - Platform-specific hashtag usage patterns
- **Mention Formatting** - @ mentions with platform-appropriate syntax

### Link-Back Strategies
- **Traffic Driver** - Include blog links in social posts to drive readership when applicable
- **Call-to-Action** - Encourage engagement with "Read the full article" messaging when applicable
- **UTM Tracking** - Add platform-specific tracking parameters to measure referral traffic
- **Custom Short URLs** - Use branded or trackable short links

## Content Synchronization & Divergence

### Synchronized Content Model
By default, when content is published to multiple platforms simultaneously, it maintains a **synchronized state**. This means:
- Changes made to the master content propagate to all connected platforms
- Updates are applied uniformly across all channels
- Deletion of master content can optionally remove from all platforms
- Metadata changes (tags, visibility) sync across platforms where applicable

### Platform Divergence
Once a **platform-specific edit** is made to content on a particular social platform, that platform's content becomes **diverged** and independent:

#### Divergence Triggers
Content becomes platform-specific when:
- User manually edits the post on a specific platform after initial publication
- Platform-specific comments or replies are added that don't apply elsewhere
- Media is replaced or adjusted for that platform only
- Platform-specific metadata (location tags, product tags, etc.) is added

#### Post-Divergence Behavior
After divergence occurs:
- **Independence** - The diverged platform content operates independently
- **No Sync** - Updates to master content no longer affect the diverged version
- **Visual Indicator** - UI shows which platforms have diverged content
- **Manual Sync Option** - Users can manually re-sync to master, overwriting changes
- **Historical Tracking** - System tracks divergence point and reason

#### Use Cases for Divergence
- **A/B Testing** - Test different messaging on different platforms
- **Platform Optimization** - Tailor content to each platform's best practices after initial publication
- **Audience Customization** - Adjust tone or details based on platform demographics
- **Crisis Management** - Modify or remove content from specific platforms without affecting others
- **Engagement Response** - Update posts on specific platforms based on audience feedback

#### Re-Synchronization
Users can choose to re-synchronize diverged content:
- **Overwrite with Master** - Discard platform-specific changes and restore sync
- **Promote Platform Version** - Make the diverged version the new master for other platforms
- **Merge Changes** - Manually review and combine changes from multiple versions (advanced)

## Content Scheduling & Calendar

### Unified Content Calendar
A centralized calendar displays:
- **Blog Posts** - Scheduled and published blog articles
- **Social Posts** - Cross-platform social media content
- **Campaign View** - Group related content across multiple platforms
- **Color Coding** - Visual distinction between content types and platforms
- **Drag-and-Drop** - Easy rescheduling of content

### Scheduling Strategies
- **Sequential Publishing** - Publish blog first, then staggered social announcements
- **Coordinated Launches** - Simultaneous multi-platform releases
- **Drip Campaigns** - Space out related content over time
- **Evergreen Rotation** - Automatically reshare high-performing content

### Time Zone Management
- **Global Audience Support** - Schedule posts in optimal time zones per platform
- **Local Time Scheduling** - Target audience's local hours for maximum engagement
- **Multi-Region Campaigns** - Coordinate worldwide launches with regional timing

### Queue Management
- **Optimal Posting Times** - AI-suggested best times based on audience activity
- **Queue Fill** - Maintain consistent posting frequency automatically
- **Priority Scheduling** - Time-sensitive content jumps the queue
- **Buffer Management** - Maintain a cushion of pre-scheduled content

## Analytics & Engagement Tracking

### Unified Dashboard
- **Cross-Platform Metrics** - Compare performance across all channels
- **Blog Traffic Analysis** - Page views, time on page, bounce rates
- **Social Engagement** - Likes, shares, comments, clicks across platforms
- **Referral Tracking** - Monitor social-to-blog traffic flow
- **Conversion Metrics** - Track content performance against business goals

### Key Performance Indicators
- **Reach & Impressions** - How many people saw the content
- **Engagement Rate** - Percentage of audience interacting with content
- **Click-Through Rate** - Effectiveness of driving traffic to blog
- **Audience Growth** - Follower/subscriber growth attributed to content
- **Best Performing Platforms** - Identify which channels deliver best ROI

### Actionable Insights
- **Content Recommendations** - Suggest topics based on past performance
- **Optimal Posting Times** - Data-driven scheduling suggestions
- **Hashtag Performance** - Which tags drive the most engagement
- **Content Type Analysis** - Compare performance of text, image, video posts

## Media Management

### Shared Media Library
- **Centralized Storage** - Single repository for all images, videos, and documents
- **Asset Organization** - Tag and categorize media for easy discovery
- **Version Control** - Track edits and maintain original versions
- **Usage Tracking** - See where each asset has been used

### Platform-Specific Requirements
Automatic optimization for each platform:

#### Image Specifications
- **X** - 1200x675px (16:9) for optimal timeline display
- **Instagram** - 1080x1080px (1:1) for feed, 1080x1920px (9:16) for stories
- **Facebook** - 1200x630px for link previews
- **LinkedIn** - 1200x627px for shared content

#### Video Specifications
- **Length Limits** - X (2:20), Instagram Feed (60s), Reels (90s), LinkedIn (10min)
- **File Size** - Platform-specific maximum file sizes
- **Format Support** - MP4, MOV, and other supported codecs

### Automatic Optimization
- **Intelligent Cropping** - Focus on important areas when adapting aspect ratios
- **Compression** - Reduce file size while maintaining quality
- **Format Conversion** - Convert between supported formats as needed
- **Batch Processing** - Prepare media for multiple platforms simultaneously

### Accessibility
- **Alt Text Management** - Add descriptive text for screen readers
- **Caption Generation** - Auto-generate or manually add video captions
- **Accessibility Audit** - Check content for accessibility compliance

## Authentication & Authorization

### OAuth Integration
Each platform requires its own authentication flow:
- **Facebook** - Facebook Login with required permissions
- **X** - OAuth 2.0 with Twitter API v2
- **Instagram** - Facebook/Instagram Graph API authentication
- **LinkedIn** - OAuth 2.0 with LinkedIn API
- **BlueSky** - App-specific password or OAuth
- **Mastodon** - Instance-specific OAuth
- **Reddit** - OAuth 2.0 with required scopes

### Token Management
- **Secure Storage** - Encrypted storage of access tokens and secrets
- **Automatic Refresh** - Handle token expiration and refresh automatically
- **Expiration Monitoring** - Alert users when re-authentication is needed
- **Revocation Handling** - Gracefully handle revoked permissions

### Multi-Account Support
- **Multiple Profiles Per Platform** - Manage several X accounts, Facebook pages, etc.
- **Account Switching** - Easy switching between connected accounts
- **Team Collaboration** - Share access with team members where supported
- **Role-Based Access** - Different permission levels for team members

### Required Permission Scopes

#### Facebook
- `pages_manage_posts` - Publish to pages
- `pages_read_engagement` - Read engagement metrics
- `pages_manage_metadata` - Manage page settings

#### X (Twitter)
- `tweet.read` - Read tweets and user info
- `tweet.write` - Create and delete tweets
- `users.read` - Read user profile information

#### Instagram
- `instagram_basic` - Basic profile access
- `instagram_content_publish` - Publish photos and videos
- `pages_read_engagement` - Read insights

#### LinkedIn
- `w_member_social` - Post, comment, and share on LinkedIn
- `r_basicprofile` - Read basic profile information
- `r_organization_social` - Read organization pages

## Platform-Specific Features

### X (Twitter)
- **Threads** - Create connected series of tweets
- **Polls** - Interactive polls with up to 4 options
- **Quote Tweets** - Share others' content with commentary
- **Spaces** - Audio conversations (integration TBD)
- **Communities** - Post to specific topic-based communities

### Instagram
- **Feed Posts** - Standard single or carousel posts
- **Stories** - 24-hour ephemeral content
- **Reels** - Short-form video content
- **IGTV** - Long-form video content
- **Shopping Tags** - Tag products in posts (if applicable)

### LinkedIn
- **Articles** - Long-form native LinkedIn articles
- **Polls** - Professional polling with up to 4 options
- **Document Sharing** - Upload PDFs and presentations
- **Newsletter Publishing** - Create and distribute newsletters
- **Company Page Updates** - Post as organization

### Facebook
- **Page Posts** - Standard posts to Facebook pages
- **Stories** - 24-hour ephemeral content
- **Events** - Create and promote events
- **Live Video** - Stream live video content
- **Groups** - Post to Facebook groups (with permission)

### Reddit
- **Subreddit Rules** - Comply with community-specific rules
- **Flair** - Add post and user flair where required
- **Crossposting** - Share posts across relevant subreddits
- **Comment Threads** - Engage in discussion threads
- **Karma Tracking** - Monitor post and comment karma

### BlueSky
- **Decentralized Protocol** - AT Protocol integration
- **Custom Feeds** - Algorithmic and chronological options
- **Moderation Lists** - Community-based moderation tools
- **Invite System** - Handle invite code management (if still applicable)

### Mastodon
- **Instance Selection** - Post to specific Mastodon instances
- **Content Warnings** - Add CW tags to sensitive content
- **Visibility Controls** - Public, unlisted, followers-only, or direct
- **Custom Emojis** - Use instance-specific emoji
- **Federated Timeline** - Cross-instance content discovery

## Future Enhancements (TODO)
- **AI Content Suggestions** - Leverage Bot integration for content recommendations
- **Sentiment Analysis** - Analyze audience response across platforms
- **Competitive Analysis** - Compare performance against competitors
- **Automated Responses** - Bot-powered replies to common questions
- **Content Performance Predictions** - AI-driven forecasting of post performance
- **Smart Hashtag Recommendations** - ML-based hashtag suggestions
- **Image Recognition** - Auto-tag and categorize images
- **Translation Support** - Multi-language content distribution

