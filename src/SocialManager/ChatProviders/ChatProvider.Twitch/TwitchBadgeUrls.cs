namespace ChatProvider.Twitch;

/// <summary>
/// Helper class for managing Twitch badge URLs
/// Note: In a production environment, you should fetch these from the Twitch API
/// using the Global Badges API: https://dev.twitch.tv/docs/api/reference#get-global-chat-badges
/// and Channel Badges API: https://dev.twitch.tv/docs/api/reference#get-channel-chat-badges
/// </summary>
public static class TwitchBadgeUrls
{
    /// <summary>
    /// Default global badge URLs (these are static CDN URLs)
    /// </summary>
    private static readonly Dictionary<string, string> GlobalBadges = new()
    {
        { "moderator", "https://static-cdn.jtvnw.net/badges/v1/3267646d-33f0-4b17-b3df-f923a41db1d0/1" },
        { "vip", "https://static-cdn.jtvnw.net/badges/v1/b817aba4-fad8-49e2-b88a-7cc744dfa6ec/1" },
        { "broadcaster", "https://static-cdn.jtvnw.net/badges/v1/5527c58c-fb7d-422d-b71b-f309dcb85cc1/1" },
        { "partner", "https://static-cdn.jtvnw.net/badges/v1/d12a2e27-16f6-41d0-ab77-b780518f00a3/1" },
        { "turbo", "https://static-cdn.jtvnw.net/badges/v1/bd444ec6-8f34-4bf9-91f4-af1e3428d80f/1" },
        { "premium", "https://static-cdn.jtvnw.net/badges/v1/bbbe0db0-a598-423e-86d0-f9fb98ca1933/1" },
        { "staff", "https://static-cdn.jtvnw.net/badges/v1/d97c37bd-a6f5-4c38-8f57-4e4bef88af34/1" },
        { "admin", "https://static-cdn.jtvnw.net/badges/v1/9ef7e029-4cdf-4d4d-a0d5-e2b3fb2ef9ef/1" },
        { "global_mod", "https://static-cdn.jtvnw.net/badges/v1/9384c43e-4ce7-4e94-b2a1-b93656896eba/1" }
    };

    /// <summary>
    /// Gets the badge URL for a given badge name and version
    /// </summary>
    /// <param name="badgeName">The name of the badge (e.g., "moderator", "subscriber")</param>
    /// <param name="version">The version of the badge</param>
    /// <returns>The URL to the badge image, or null if not found</returns>
    public static string? GetBadgeUrl(string badgeName, string version)
    {
        // For subscriber badges, the version indicates the subscription length
        // These URLs need to be fetched per-channel from the Twitch API
        if (badgeName == "subscriber")
        {
            // This is a placeholder - in production, fetch from Twitch API per channel
            return $"https://static-cdn.jtvnw.net/badges/v1/5d9f2208-5dd8-11e7-8513-2ff4adfae661/{version}";
        }

        // Return global badge URL if available
        if (GlobalBadges.TryGetValue(badgeName, out var url))
        {
            return url;
        }

        return null;
    }

    /// <summary>
    /// Generates CSS background-image rules for all known badges
    /// This can be used to generate dynamic CSS
    /// </summary>
    /// <returns>CSS rules for badge background images</returns>
    public static string GenerateBadgeCss()
    {
        var css = new System.Text.StringBuilder();
        
        foreach (var (badgeName, url) in GlobalBadges)
        {
            css.AppendLine($".twitch-badge-{badgeName} {{");
            css.AppendLine($"    background-image: url('{url}');");
            css.AppendLine("}");
            css.AppendLine();
        }

        return css.ToString();
    }
}
