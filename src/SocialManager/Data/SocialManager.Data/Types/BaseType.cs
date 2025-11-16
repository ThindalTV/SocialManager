namespace SocialManager.Data.Types;

public class BaseType
{
    public string? Id { get; set; }

    public bool IsPublished { get; set; } = false;

    // Soft deletes
    public bool IsDeleted { get; set; } = false;

    // Change log
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}
