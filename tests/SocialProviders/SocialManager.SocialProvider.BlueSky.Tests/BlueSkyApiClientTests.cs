using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SocialManager.SocialProvider.BlueSky.Client;
using SocialManager.SocialProvider.BlueSky.Configuration;

namespace SocialManager.SocialProvider.BlueSky.Tests;

/// <summary>
/// Unit tests for FishyFlipApiClient.
/// Note: These are structural tests. Full integration tests would require a real BlueSky account.
/// </summary>
public class BlueSkyApiClientTests
{
    private readonly Mock<ILogger<FishyFlipApiClient>> _mockLogger;

    public BlueSkyApiClientTests()
    {
        _mockLogger = new Mock<ILogger<FishyFlipApiClient>>();
    }

    [Fact]
    public void Constructor_WithNullConfig_ThrowsArgumentNullException()
    {
        var act = () => new FishyFlipApiClient(null!, _mockLogger.Object);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        var config = CreateValidConfig();
        var act = () => new FishyFlipApiClient(config, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithInvalidConfig_ThrowsInvalidOperationException()
    {
        var invalidConfig = Options.Create(new BlueSkyProviderConfiguration
        {
            Identifier = "",  // Invalid - empty
            AppPassword = "test-password",
            PdsUrl = "https://bsky.social"
        });

        var act = () => new FishyFlipApiClient(invalidConfig, _mockLogger.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Identifier*required*");
    }

    [Fact]
    public void Constructor_WithValidConfig_DoesNotThrow()
    {
        var config = CreateValidConfig();
        var act = () => new FishyFlipApiClient(config, _mockLogger.Object);
        act.Should().NotThrow();
    }

    [Fact]
    public async Task PublishPostAsync_WithNullOrEmptyText_ThrowsArgumentException()
    {
        var client = CreateClient();

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.PublishPostAsync("", CancellationToken.None));

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.PublishPostAsync("   ", CancellationToken.None));
    }

    [Fact]
    public async Task PublishPostWithMediaAsync_WithNullMediaData_ThrowsArgumentNullException()
    {
        var client = CreateClient();

        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.PublishPostWithMediaAsync("Test", null!, "image/jpeg", CancellationToken.None));
    }

    [Fact]
    public async Task PublishPostWithMediaAsync_WithNullOrEmptyMimeType_ThrowsArgumentException()
    {
        var client = CreateClient();
        var mediaData = new byte[] { 1, 2, 3 };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.PublishPostWithMediaAsync("Test", mediaData, "", CancellationToken.None));

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.PublishPostWithMediaAsync("Test", mediaData, "   ", CancellationToken.None));
    }

    [Fact]
    public async Task GetPostAsync_WithNullOrEmptyUri_ThrowsArgumentException()
    {
        var client = CreateClient();

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.GetPostAsync("", CancellationToken.None));

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.GetPostAsync("   ", CancellationToken.None));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Configuration_WithMissingIdentifier_FailsValidation(string identifier)
    {
        var config = new BlueSkyProviderConfiguration
        {
            Identifier = identifier,
            AppPassword = "test-password",
            PdsUrl = "https://bsky.social"
        };

        var act = () => config.Validate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Identifier*required*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Configuration_WithMissingAppPassword_FailsValidation(string appPassword)
    {
        var config = new BlueSkyProviderConfiguration
        {
            Identifier = "test.bsky.social",
            AppPassword = appPassword,
            PdsUrl = "https://bsky.social"
        };

        var act = () => config.Validate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*AppPassword*required*");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Configuration_WithNegativeMaxRetryAttempts_FailsValidation(int maxRetry)
    {
        var config = new BlueSkyProviderConfiguration
        {
            Identifier = "test.bsky.social",
            AppPassword = "test-password",
            PdsUrl = "https://bsky.social",
            MaxRetryAttempts = maxRetry
        };

        var act = () => config.Validate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*MaxRetryAttempts*non-negative*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-30)]
    public void Configuration_WithInvalidRequestTimeout_FailsValidation(int timeout)
    {
        var config = new BlueSkyProviderConfiguration
        {
            Identifier = "test.bsky.social",
            AppPassword = "test-password",
            PdsUrl = "https://bsky.social",
            RequestTimeoutSeconds = timeout
        };

        var act = () => config.Validate();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*RequestTimeoutSeconds*positive*");
    }

    private static IOptions<BlueSkyProviderConfiguration> CreateValidConfig()
    {
        return Options.Create(new BlueSkyProviderConfiguration
        {
            Active = true,
            Platform = "BlueSky",
            Identifier = "test.bsky.social",
            AppPassword = "test-app-password",
            PdsUrl = "https://bsky.social",
            EnableRetryOnRateLimit = true,
            MaxRetryAttempts = 3,
            RequestTimeoutSeconds = 30
        });
    }

    private FishyFlipApiClient CreateClient()
    {
        var config = CreateValidConfig();
        return new FishyFlipApiClient(config, _mockLogger.Object);
    }
}
