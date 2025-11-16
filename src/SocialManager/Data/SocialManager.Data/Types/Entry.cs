using SocialManager.Data.Types.Blog;
using SocialManager.Data.Types.Social;

namespace SocialManager.Data.Types;

public class Entry : BaseType
{
    public BlogPost? BlogPost { get; set; }

    public List<SocialPost> SocialPosts { get; set; } = [];
}
