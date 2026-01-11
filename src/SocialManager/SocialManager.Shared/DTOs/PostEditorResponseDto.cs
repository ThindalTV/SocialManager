namespace SocialManager.Shared.DTOs;

/// <summary>
/// Response DTO for the post editor page
/// </summary>
public class PostEditorResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string SharedSocialText { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    
    /// <summary>
    /// Gets or sets the scheduled publish date. If null, publishes immediately when IsPublished is true.
    /// </summary>
    public DateTimeOffset? PublishDate { get; set; }
    
    public List<string> Tags { get; set; } = [];
    public string? Category { get; set; }
    public List<PlatformPostDto> PlatformPosts { get; set; } = [];
}

/// <summary>
/// DTO for platform-specific post configuration.
/// All available platforms should always be present in the list, regardless of whether they have custom content or are enabled.
/// </summary>
public class PlatformPostDto
{
    /// <summary>
    /// Gets or sets the social media platform name (e.g., "X", "LinkedIn", "Facebook")
    /// </summary>
    public string Platform { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether this platform is enabled for posting
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// Gets or sets the custom text for this platform. If empty, the shared social text will be used.
    /// </summary>
    public string CustomText { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the character limit for this platform
    /// </summary>
    public int CharacterLimit { get; set; }
}
