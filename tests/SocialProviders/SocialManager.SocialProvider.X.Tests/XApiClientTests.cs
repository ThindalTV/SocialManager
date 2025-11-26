using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SocialManager.SocialProvider.X.Client;
using SocialManager.SocialProvider.X.Configuration;

namespace SocialManager.SocialProvider.X.Tests;

public class XApiClientTests
{
    private readonly Mock<ILogger<LinqToTwitterApiClient>> _mockLogger;
    private readonly XProviderConfiguration _validConfiguration;

    public XApiClientTests()
    {
        _mockLogger = new Mock<ILogger<LinqToTwitterApiClient>>();
        _validConfiguration = new XProviderConfiguration
        {
            ApiKey = "test-api-key",
            ApiSecret = "test-api-secret",
            AccessToken = "test-access-token",
            AccessTokenSecret = "test-access-token-secret"
        };
    }

    [Fact]
    public void Constructor_WithValidConfiguration_CreatesClient()
    {
        var options = Options.Create(_validConfiguration);

        var client = new LinqToTwitterApiClient(options, _mockLogger.Object);

        client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithMissingApiKey_ThrowsInvalidOperationException()
    {
        var invalidConfig = new XProviderConfiguration
        {
            ApiKey = "",
            ApiSecret = "test-secret",
            AccessToken = "test-token",
            AccessTokenSecret = "test-token-secret"
        };
        var options = Options.Create(invalidConfig);

        var act = () => new LinqToTwitterApiClient(options, _mockLogger.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*ApiKey*required*");
    }

    [Fact]
    public void Constructor_WithMissingApiSecret_ThrowsInvalidOperationException()
    {
        var invalidConfig = new XProviderConfiguration
        {
            ApiKey = "test-key",
            ApiSecret = "",
            AccessToken = "test-token",
            AccessTokenSecret = "test-token-secret"
        };
        var options = Options.Create(invalidConfig);

        var act = () => new LinqToTwitterApiClient(options, _mockLogger.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*ApiSecret*required*");
    }

    [Fact]
    public void Constructor_WithMissingAccessToken_ThrowsInvalidOperationException()
    {
        var invalidConfig = new XProviderConfiguration
        {
            ApiKey = "test-key",
            ApiSecret = "test-secret",
            AccessToken = "",
            AccessTokenSecret = "test-token-secret"
        };
        var options = Options.Create(invalidConfig);

        var act = () => new LinqToTwitterApiClient(options, _mockLogger.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*AccessToken*required*");
    }

    [Fact]
    public void Constructor_WithMissingAccessTokenSecret_ThrowsInvalidOperationException()
    {
        var invalidConfig = new XProviderConfiguration
        {
            ApiKey = "test-key",
            ApiSecret = "test-secret",
            AccessToken = "test-token",
            AccessTokenSecret = ""
        };
        var options = Options.Create(invalidConfig);

        var act = () => new LinqToTwitterApiClient(options, _mockLogger.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*AccessTokenSecret*required*");
    }
}
