using ChatProvider.Twitch;

namespace SocialManager.Tests.ChatProviders;

public class TwitchBadgeUrlsTests
{
    [Theory]
    [InlineData("moderator", "1")]
    [InlineData("vip", "1")]
    [InlineData("broadcaster", "1")]
    [InlineData("partner", "1")]
    [InlineData("turbo", "1")]
    [InlineData("premium", "1")]
    [InlineData("staff", "1")]
    [InlineData("admin", "1")]
    [InlineData("global_mod", "1")]
    public void GetBadgeUrl_WithKnownBadge_ShouldReturnValidUrl(string badgeName, string version)
    {
        // Act
        var url = TwitchBadgeUrls.GetBadgeUrl(badgeName, version);

        // Assert
        Assert.NotNull(url);
        Assert.Contains("static-cdn.jtvnw.net", url);
        Assert.Contains("badges/v1/", url);
    }

    [Fact]
    public void GetBadgeUrl_WithSubscriberBadge_ShouldReturnVersionedUrl()
    {
        // Arrange
        var version = "12";

        // Act
        var url = TwitchBadgeUrls.GetBadgeUrl("subscriber", version);

        // Assert
        Assert.NotNull(url);
        Assert.Contains("static-cdn.jtvnw.net", url);
        Assert.Contains(version, url);
    }

    [Fact]
    public void GetBadgeUrl_WithUnknownBadge_ShouldReturnNull()
    {
        // Act
        var url = TwitchBadgeUrls.GetBadgeUrl("unknown_badge_type", "1");

        // Assert
        Assert.Null(url);
    }

    [Theory]
    [InlineData("moderator")]
    [InlineData("vip")]
    [InlineData("broadcaster")]
    public void GetBadgeUrl_ShouldReturnHttpsUrl(string badgeName)
    {
        // Act
        var url = TwitchBadgeUrls.GetBadgeUrl(badgeName, "1");

        // Assert
        Assert.NotNull(url);
        Assert.StartsWith("https://", url);
    }

    [Fact]
    public void GenerateBadgeCss_ShouldReturnValidCss()
    {
        // Act
        var css = TwitchBadgeUrls.GenerateBadgeCss();

        // Assert
        Assert.NotEmpty(css);
        Assert.Contains(".twitch-badge-", css);
        Assert.Contains("background-image:", css);
        Assert.Contains("url('", css);
    }

    [Theory]
    [InlineData("moderator")]
    [InlineData("vip")]
    [InlineData("broadcaster")]
    [InlineData("partner")]
    public void GenerateBadgeCss_ShouldContainBadgeClass(string badgeName)
    {
        // Act
        var css = TwitchBadgeUrls.GenerateBadgeCss();

        // Assert
        Assert.Contains($".twitch-badge-{badgeName}", css);
    }

    [Fact]
    public void GenerateBadgeCss_ShouldContainCdnUrls()
    {
        // Act
        var css = TwitchBadgeUrls.GenerateBadgeCss();

        // Assert
        Assert.Contains("static-cdn.jtvnw.net", css);
    }

    [Fact]
    public void GenerateBadgeCss_ShouldHaveProperCssStructure()
    {
        // Act
        var css = TwitchBadgeUrls.GenerateBadgeCss();

        // Assert
        Assert.Contains("{", css);
        Assert.Contains("}", css);
        Assert.Contains(";", css);
    }
}
