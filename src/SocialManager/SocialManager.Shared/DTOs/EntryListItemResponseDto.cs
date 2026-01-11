namespace SocialManager.Shared.DTOs;

/// <summary>
/// Response DTO for displaying entry list items in the grid
/// </summary>
public class EntryListItemResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public bool IsPublished { get; set; }
    
    // Platform indicators
    public List<string> SocialPlatforms { get; set; } = [];
}
