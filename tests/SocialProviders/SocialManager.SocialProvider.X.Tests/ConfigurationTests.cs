using FluentAssertions;
using SocialManager.SocialProvider.X.Configuration;

namespace SocialManager.SocialProvider.X.Tests;

public class ConfigurationTests
{
    [Fact]
    public void Validate_WithValidConfiguration_DoesNotThrow()
    {
        var config = new XProviderConfiguration
        {
            ApiKey = "test-key",
            ApiSecret = "test-secret",
            AccessToken = "test-token",
            AccessTokenSecret = "test-token-secret"
        };

        var act = () => config.Validate();
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithMissingApiKey_ThrowsInvalidOperationException()
    {
        var config = new XProviderConfiguration
        {
            ApiKey = "",
            ApiSecret = "test-secret",
            AccessToken = "test-token",
            AccessTokenSecret = "test-token-secret"
        };

        var act = () => config.Validate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*ApiKey*required*");
    }

    [Fact]
    public void Validate_WithWhitespaceApiKey_ThrowsInvalidOperationException()
    {
        var config = new XProviderConfiguration
        {
            ApiKey = "   ",
            ApiSecret = "test-secret",
            AccessToken = "test-token",
            AccessTokenSecret = "test-token-secret"
        };

        var act = () => config.Validate();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Validate_WithNegativeMaxRetryAttempts_ThrowsInvalidOperationException()
    {
        var config = new XProviderConfiguration
        {
            ApiKey = "test-key",
            ApiSecret = "test-secret",
            AccessToken = "test-token",
            AccessTokenSecret = "test-token-secret",
            MaxRetryAttempts = -1
        };

        var act = () => config.Validate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*MaxRetryAttempts*non-negative*");
    }

    [Fact]
    public void SectionName_HasCorrectValue()
    {
        XProviderConfiguration.SectionName.Should().Be("XProvider");
    }

    [Fact]
    public void DefaultValues_AreSetCorrectly()
    {
        var config = new XProviderConfiguration();

        config.EnableRetryOnRateLimit.Should().BeTrue();
        config.MaxRetryAttempts.Should().Be(3);
        config.ApiKey.Should().BeEmpty();
        config.ApiSecret.Should().BeEmpty();
        config.AccessToken.Should().BeEmpty();
        config.AccessTokenSecret.Should().BeEmpty();
        config.BearerToken.Should().BeNull();
        config.Active.Should().BeTrue();
        config.Platform.Should().Be("X");
    }

    [Fact]
    public void ISocialProviderConfiguration_PropertiesAreImplemented()
    {
        var config = new XProviderConfiguration
        {
            Active = false,
            Platform = "Twitter"
        };

        config.Active.Should().BeFalse();
        config.Platform.Should().Be("Twitter");
    }
}
