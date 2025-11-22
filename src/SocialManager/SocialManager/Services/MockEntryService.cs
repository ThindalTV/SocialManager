using SocialManager.Shared;
using SocialManager.Shared.DTOs;

namespace SocialManager.Services;

/// <summary>
/// Mock implementation of IEntryService for development purposes
/// This will be replaced with actual API calls
/// </summary>
public class MockEntryService : IEntryService
{
    private readonly List<PostEditorResponseDto> _mockEntries = [];
    private int _nextId = 1;

    public MockEntryService()
    {
        // Initialize with some mock data
        _mockEntries.AddRange([
            new PostEditorResponseDto
            {
                Id = (_nextId++).ToString(),
                Title = "Getting Started with Social Media Management",
                Synopsis = "A comprehensive guide to help you get started with managing your social media presence effectively.",
                BlogContent = "<p>Welcome to our comprehensive guide on social media management...</p>",
                SharedSocialText = "Check out our new guide on social media management! #SocialMedia #Management",
                IsPublished = true,
                Tags = ["Social Media", "Management", "Guide"],
                Category = "Marketing",
                PlatformPosts = GetDefaultPlatformPosts()
            },
            new PostEditorResponseDto
            {
                Id = (_nextId++).ToString(),
                Title = "10 Tips for Better Content Creation",
                Synopsis = "Discover ten essential tips that will transform your content creation process and boost engagement.",
                BlogContent = "<p>Content creation is an art...</p>",
                SharedSocialText = "10 essential tips for creating amazing content! 🚀",
                IsPublished = false,
                Tags = ["Content Creation", "Tips", "Tutorial"],
                Category = "Development",
                PlatformPosts = GetDefaultPlatformPosts()
            },
            new PostEditorResponseDto
            {
                Id = (_nextId++).ToString(),
                Title = "The Future of Social Media Platforms",
                Synopsis = "An in-depth look at emerging trends and technologies shaping the future of social media.",
                BlogContent = "<p>As we look ahead to the future...</p>",
                SharedSocialText = "Exploring the future of social media platforms",
                IsPublished = true,
                Tags = ["Social Media", "Future", "Technology"],
                Category = "Opinion",
                PlatformPosts = GetDefaultPlatformPosts()
            }
        ]);
    }

    public async Task<(List<EntryListItemResponseDto> Items, int TotalCount)> GetEntriesAsync(
        int page, 
        int pageSize, 
        string? sortField = null, 
        string? sortDirection = null, 
        string? filterText = null)
    {
        await Task.Delay(100); // Simulate API call

        var query = _mockEntries.AsQueryable();

        // Apply filter
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            query = query.Where(e => e.Title.Contains(filterText, StringComparison.OrdinalIgnoreCase));
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(sortField))
        {
            query = sortField switch
            {
                "Title" => sortDirection == "asc" ? query.OrderBy(e => e.Title) : query.OrderByDescending(e => e.Title),
                "IsPublished" => sortDirection == "asc" ? query.OrderBy(e => e.IsPublished) : query.OrderByDescending(e => e.IsPublished),
                _ => query
            };
        }

        var totalCount = query.Count();
        
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EntryListItemResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                CreatedDate = DateTimeOffset.UtcNow.AddDays(-int.Parse(e.Id)), // Mock date
                UpdatedDate = DateTimeOffset.UtcNow.AddHours(-int.Parse(e.Id)), // Mock date
                IsPublished = e.IsPublished,
                HasBlogPost = !string.IsNullOrWhiteSpace(e.BlogContent),
                SocialPlatforms = e.PlatformPosts.Where(p => p.IsEnabled).Select(p => p.Platform).ToList()
            })
            .ToList();

        return (items, totalCount);
    }

    public async Task<PostEditorResponseDto?> GetEntryAsync(string id)
    {
        await Task.Delay(50); // Simulate API call
        return _mockEntries.FirstOrDefault(e => e.Id == id);
    }

    public async Task<string> CreateEntryAsync(PostEditorResponseDto entry)
    {
        await Task.Delay(100); // Simulate API call
        
        entry.Id = (_nextId++).ToString();
        _mockEntries.Add(entry);
        
        return entry.Id;
    }

    public async Task<bool> UpdateEntryAsync(PostEditorResponseDto entry)
    {
        await Task.Delay(100); // Simulate API call
        
        var existing = _mockEntries.FirstOrDefault(e => e.Id == entry.Id);
        if (existing == null) return false;

        var index = _mockEntries.IndexOf(existing);
        _mockEntries[index] = entry;
        
        return true;
    }

    public async Task<bool> DeleteEntryAsync(string id)
    {
        await Task.Delay(100); // Simulate API call
        
        var entry = _mockEntries.FirstOrDefault(e => e.Id == id);
        if (entry == null) return false;

        _mockEntries.Remove(entry);
        return true;
    }

    private static List<PlatformPostDto> GetDefaultPlatformPosts()
    {
        return [
            new() { Platform = SocialPlatforms.X, IsEnabled = true, CharacterLimit = 280 },
            new() { Platform = SocialPlatforms.BlueSky, IsEnabled = true, CharacterLimit = 300 },
            new() { Platform = SocialPlatforms.Mastodon, IsEnabled = false, CharacterLimit = 500 },
            new() { Platform = SocialPlatforms.LinkedIn, IsEnabled = true, CharacterLimit = 3000 },
            new() { Platform = SocialPlatforms.Facebook, IsEnabled = false, CharacterLimit = 63206 },
            new() { Platform = SocialPlatforms.Instagram, IsEnabled = false, CharacterLimit = 2200 },
            new() { Platform = SocialPlatforms.TikTok, IsEnabled = false, CharacterLimit = 2200 },
            new() { Platform = SocialPlatforms.Pinterest, IsEnabled = false, CharacterLimit = 500 },
            new() { Platform = SocialPlatforms.Reddit, IsEnabled = false, CharacterLimit = 40000 },
            new() { Platform = SocialPlatforms.Thread, IsEnabled = false, CharacterLimit = 500 }
        ];
    }
}
