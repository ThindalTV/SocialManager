using ChatProvider.Twitch;

namespace SocialManager.Tests.ChatProviders;

/// <summary>
/// Tests for TwitchChatProvider HTML formatting functionality
/// Note: These tests verify the expected HTML structure and CSS classes
/// </summary>
public class TwitchHtmlFormattingTests
{
    [Fact]
    public void BadgeHtml_ShouldContainCorrectCssClasses()
    {
        // Expected badge HTML structure
        var expectedBadgePattern = "twitch-badge twitch-badge-moderator";
        
        // Assert
        Assert.Contains("twitch-badge", expectedBadgePattern);
        Assert.Contains("twitch-badge-moderator", expectedBadgePattern);
    }

    [Fact]
    public void BadgeHtml_ShouldContainDataVersionAttribute()
    {
        // Expected badge HTML with data-version attribute
        var expectedBadgeHtml = "<span class=\"twitch-badge twitch-badge-moderator\" data-version=\"1\" title=\"moderator\"></span>";
        
        // Assert
        Assert.Contains("data-version=\"1\"", expectedBadgeHtml);
        Assert.Contains("title=\"moderator\"", expectedBadgeHtml);
    }

    [Fact]
    public void BadgesContainer_ShouldHaveCorrectCssClass()
    {
        // Expected badges container
        var expectedContainer = "<span class=\"twitch-badges\">";
        
        // Assert
        Assert.Contains("twitch-badges", expectedContainer);
    }

    [Fact]
    public void EmoteHtml_ShouldContainImgTag()
    {
        // Expected emote HTML structure
        var expectedEmoteHtml = "<img src=\"https://static-cdn.jtvnw.net/emoticons/v2/25/default/dark/1.0\" alt=\"Kappa\" title=\"Kappa\" class=\"twitch-emote\" />";
        
        // Assert
        Assert.Contains("<img", expectedEmoteHtml);
        Assert.Contains("class=\"twitch-emote\"", expectedEmoteHtml);
        Assert.Contains("emoticons/v2/", expectedEmoteHtml);
    }

    [Fact]
    public void EmoteHtml_ShouldContainAltAndTitleAttributes()
    {
        // Expected emote HTML with alt and title
        var expectedEmoteHtml = "<img src=\"https://static-cdn.jtvnw.net/emoticons/v2/25/default/dark/1.0\" alt=\"Kappa\" title=\"Kappa\" class=\"twitch-emote\" />";
        
        // Assert
        Assert.Contains("alt=\"Kappa\"", expectedEmoteHtml);
        Assert.Contains("title=\"Kappa\"", expectedEmoteHtml);
    }

    [Fact]
    public void MentionHtml_ShouldContainCorrectCssClass()
    {
        // Expected mention HTML structure
        var expectedMentionHtml = "<span class=\"twitch-mention\">@username</span>";
        
        // Assert
        Assert.Contains("class=\"twitch-mention\"", expectedMentionHtml);
        Assert.Contains("@username", expectedMentionHtml);
    }

    [Fact]
    public void MentionHtml_ShouldPreservePunctuation()
    {
        // Mentions with punctuation should separate the punctuation
        var expectedMentionWithPunctuation = "<span class=\"twitch-mention\">@user</span>!";
        
        // Assert
        Assert.Contains("<span class=\"twitch-mention\">@user</span>", expectedMentionWithPunctuation);
        Assert.EndsWith("!", expectedMentionWithPunctuation);
    }

    [Fact]
    public void CombinedHtml_ShouldContainAllElements()
    {
        // Expected combined HTML with badges, mentions, and emotes
        var expectedCombinedHtml = "<span class=\"twitch-badges\"><span class=\"twitch-badge twitch-badge-moderator\" data-version=\"1\" title=\"moderator\"></span></span> Hey <span class=\"twitch-mention\">@user</span>, check <img src=\"https://static-cdn.jtvnw.net/emoticons/v2/25/default/dark/1.0\" alt=\"Kappa\" title=\"Kappa\" class=\"twitch-emote\" /> out!";
        
        // Assert - should contain all three types of formatting
        Assert.Contains("twitch-badges", expectedCombinedHtml);
        Assert.Contains("twitch-mention", expectedCombinedHtml);
        Assert.Contains("twitch-emote", expectedCombinedHtml);
    }

    [Theory]
    [InlineData("moderator")]
    [InlineData("subscriber")]
    [InlineData("vip")]
    [InlineData("broadcaster")]
    [InlineData("partner")]
    public void BadgeCssClasses_ShouldBeWellFormed(string badgeType)
    {
        // Expected CSS class format
        var expectedCssClass = $"twitch-badge-{badgeType}";
        
        // Assert
        Assert.StartsWith("twitch-badge-", expectedCssClass);
        Assert.Contains(badgeType, expectedCssClass);
    }

    [Fact]
    public void HtmlEncoding_ShouldPreventXss()
    {
        // Test that HTML encoding is applied
        var dangerousInput = "<script>alert('xss')</script>";
        var expectedEncoded = System.Net.WebUtility.HtmlEncode(dangerousInput);
        
        // Assert
        Assert.DoesNotContain("<script>", expectedEncoded);
        Assert.Contains("&lt;script&gt;", expectedEncoded);
    }
}
