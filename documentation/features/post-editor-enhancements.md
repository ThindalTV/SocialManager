# Post Editor Enhancement Summary

## Overview
Added the ability to enable/disable blog posts and social posts separately, with validation to ensure at least one content type is enabled with content. Also added a publish date picker for scheduling posts.

## Changes Made

### 1. Data Transfer Object (DTO) Updates
**File:** `src\SocialManager\SocialManager.Shared\DTOs\PostEditorResponseDto.cs`

Added three new properties:
- `EnableBlogPost` (bool) - Controls whether the blog post should be created/posted (default: true)
- `EnableSocialPosts` (bool) - Controls whether social media posts should be created/posted (default: true)
- `PublishDate` (DateTimeOffset?) - Optional scheduled publish date. If null, publishes immediately when IsPublished is true.

### 2. Post Editor UI Updates
**File:** `src\SocialManager\SocialManager\Modules\Social\PostEditor.razor`

#### New Features:
1. **Publishing Options Section**
   - Two toggle switches (TelerikSwitch) for enabling/disabling blog posts and social posts
   - Date/time picker (TelerikDateTimePicker) for scheduling publish date
   - Helper text showing when the post will be published

2. **Validation Logic**
   - Added `CanSave` property that enforces business rules:
     - At least one content type (blog or social) must be enabled
     - If blog post is enabled, blog content must not be empty
     - If social posts are enabled, must have either shared text or at least one platform enabled
   - Save and Publish buttons are disabled when validation fails
   - Visual error message displays validation requirements

3. **Visual Feedback**
   - Blog editor shows "Blog post disabled" badge when disabled
   - Social editor shows "Social Posts Disabled" overlay when disabled
   - Disabled content areas are visually dimmed and non-interactive

### 3. CSS Styling Updates
**File:** `src\SocialManager\SocialManager\Modules\Social\PostEditor.razor.css`

Added styles for:
- `.publishing-options` - Container for the new publishing controls section
- `.content-toggles` - Layout for the enable/disable switches
- `.publish-date-section` - Layout for the date picker and helper text
- `.date-helper` - Styling for the date picker helper text
- `.validation-error` - Red alert box for validation error messages
- `.disabled-badge-small` - Small badge shown on disabled blog editor
- `.content-disabled` - Visual styling for disabled blog editor column
- Updated `.social-column-disabled` to properly handle the social posts disabled state

## User Experience Flow

### Saving an Entry
1. User must enable at least one content type (Blog Post or Social Posts)
2. Enabled content types must have content:
   - Blog Post: Must have blog content in the editor
   - Social Posts: Must have shared text OR at least one platform enabled
3. Save button is disabled until validation passes
4. Visual error message explains why save is disabled

### Publishing Options
1. User can independently toggle blog posts and social posts on/off
2. User can optionally set a future publish date
3. If no publish date is set, content publishes immediately when marked as "Published"
4. Disabled content areas are visually dimmed with clear indicators

### Visual States
- **Both Enabled**: Normal editing experience
- **Blog Disabled**: Blog editor shows disabled badge, social editor remains active
- **Social Disabled**: Social editor shows overlay with "Social Posts Disabled" message
- **Cannot Save**: Red error message explains validation requirements

## Technical Notes

### Validation Rules
The `CanSave` property implements the following logic:
```csharp
// Must have at least one content type enabled
if (!Model.EnableBlogPost && !Model.EnableSocialPosts) return false;

// If blog enabled, must have content
if (Model.EnableBlogPost && string.IsNullOrWhiteSpace(Model.BlogContent)) return false;

// If social enabled, must have shared text or enabled platforms
if (Model.EnableSocialPosts)
{
    var hasSharedText = !string.IsNullOrWhiteSpace(Model.SharedSocialText);
    var hasEnabledPlatform = Model.PlatformPosts?.Any(p => p.IsEnabled) ?? false;
    if (!hasSharedText && !hasEnabledPlatform) return false;
}

return true;
```

### Date/Time Handling
- Uses `DateTimeOffset?` to support nullable publish dates and time zones
- Format: "yyyy-MM-dd HH:mm" for consistent date/time display
- Null value means "publish immediately"

## Future Considerations

1. **Service Layer Updates**: The `IEntryService` implementation will need to:
   - Respect the `EnableBlogPost` and `EnableSocialPosts` flags when creating entries
   - Handle the `PublishDate` for scheduling
   - Only create blog post if `EnableBlogPost` is true
   - Only create social posts if `EnableSocialPosts` is true

2. **Scheduling System**: A background job or scheduled task will be needed to:
   - Monitor entries with future `PublishDate` values
   - Automatically publish entries when their scheduled time arrives

3. **Permissions**: May want to add role-based access control for:
   - Who can disable blog posts
   - Who can schedule posts
   - Maximum future date for scheduling

## Testing Checklist

- [ ] Can create entry with only blog post enabled
- [ ] Can create entry with only social posts enabled
- [ ] Cannot save with both content types disabled
- [ ] Cannot save blog-only entry without blog content
- [ ] Cannot save social-only entry without social text or platforms
- [ ] Can set future publish date
- [ ] Can clear publish date to publish immediately
- [ ] Visual feedback works correctly for all disabled states
- [ ] Validation error message displays correctly
- [ ] Save/Publish buttons properly disabled when validation fails
