using SocialManager.Data.Types.Blog;
using SocialManager.Data.Types.Social;

namespace SocialManager.Data.Types;

public class Entry : BaseType
{
    /// <summary>
    /// Gets or sets the title of the entry.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    public Blog.Post? BlogPost { get; set; }

    public List<Social.Post> SocialPosts { get; set; } = [];
}
