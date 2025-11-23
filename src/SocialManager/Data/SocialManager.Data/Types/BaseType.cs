namespace SocialManager.Data.Types;

/// <summary>
/// Represents the base type for entities that support publishing, soft deletion, and change tracking metadata.
/// </summary>
/// <remarks>This class provides common properties for tracking the identity, publication status, deletion state,
/// and audit information of derived entities. It is intended to be used as a foundation for domain models that require
/// these features.</remarks>
public class BaseType
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity has been published.
    /// </summary>
    /// <remarks>This property is typically set by a background task.</remarks>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the entity is a draft.
    /// </summary>
    /// <remarks>Defaults to true. When false, the entity is considered a non-draft (e.g., ready for publication).</remarks>
    public bool IsDraft { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the date and time when the entity was published.
    /// </summary>
    public DateTimeOffset? PublishedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity has been soft deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the user who last updated the entity.
    /// </summary>
    public string? UpdatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTimeOffset? CreatedDate { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedDate { get; set; }
}
