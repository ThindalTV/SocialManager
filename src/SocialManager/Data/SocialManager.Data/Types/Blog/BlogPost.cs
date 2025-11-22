namespace SocialManager.Data.Types.Blog;


/// <summary>
/// Represents a blog post entry with content, synopsis, and categorization.
/// </summary>
public class BlogPost
{
    // Blog post specific data
    

    
    /// <summary>
    /// Gets or sets a brief summary or excerpt of the blog post.
    /// </summary>
    public string Synopsis { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the textual content associated with this instance.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the raw content of the blog post in its original format (e.g., Markdown).
    /// This property is null when content is generated directly from an HTML WYSIWYG editor.
    /// </summary>
    public string? RawContent { get; set; }

    /// <summary>
    /// Gets or sets the category associated with this blog post.
    /// </summary>
    public Category? Category { get; set; }
    
    /// <summary>
    /// Gets or sets the list of tags associated with this blog post.
    /// </summary>
    public List<Tag>? Tags { get; set; }
}
