using SocialManager.Data.Types.Blog;
using SocialManager.Data.Types.Social;

namespace SocialManager.Data.Types;

public class Entry : BaseType
{
    /// <summary>
    /// Gets or sets the title of the entry.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    public BlogPost? BlogPost { get; set; }

    public List<SocialPost> SocialPosts { get; set; } = [];
}
