namespace SocialManager.Data.Types.Blog;

public class BlogPost : Entry
{
    // Blog post specific data
    public List<Category>? Categories { get; set; }
}
