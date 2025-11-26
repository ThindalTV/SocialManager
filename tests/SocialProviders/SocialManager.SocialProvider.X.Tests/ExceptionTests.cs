using FluentAssertions;
using SocialProvider.Exceptions;

namespace SocialManager.SocialProvider.X.Tests;

public class ExceptionTests
{
    [Fact]
    public void SocialProviderException_WithMessage_CreatesException()
    {
        var message = "Test error message";

        var exception = new SocialProviderException(message, "X");

        exception.Message.Should().Be(message);
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public void SocialProviderException_WithMessageAndInnerException_CreatesException()
    {
        var message = "Test error message";
        var innerException = new InvalidOperationException("Inner error");

        var exception = new SocialProviderException(message, innerException, "X");

        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public void SocialProviderException_WithNullPlatform_CreatesException()
    {
        var message = "Test error message";

        var exception = new SocialProviderException(message);

        exception.Message.Should().Be(message);
        exception.Platform.Should().BeNull();
    }

    [Fact]
    public void SocialProviderRateLimitException_WithResetTime_StoresResetTime()
    {
        var message = "Rate limit exceeded";
        var resetTime = DateTime.UtcNow.AddMinutes(15);

        var exception = new SocialProviderRateLimitException(message, resetTime, "X");

        exception.Message.Should().Be(message);
        exception.ResetTime.Should().Be(resetTime);
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public void SocialProviderRateLimitException_WithoutResetTime_HasNullResetTime()
    {
        var message = "Rate limit exceeded";

        var exception = new SocialProviderRateLimitException(message, null, "X");

        exception.Message.Should().Be(message);
        exception.ResetTime.Should().BeNull();
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public void SocialProviderAuthenticationException_WithMessage_CreatesException()
    {
        var message = "Authentication failed";

        var exception = new SocialProviderAuthenticationException(message, "X");

        exception.Message.Should().Be(message);
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public void SocialProviderAuthenticationException_WithMessageAndInnerException_CreatesException()
    {
        var message = "Authentication failed";
        var innerException = new UnauthorizedAccessException("Invalid credentials");

        var exception = new SocialProviderAuthenticationException(message, innerException, "X");

        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
        exception.Platform.Should().Be("X");
    }

    [Fact]
    public void SocialProviderRateLimitException_InheritsFromSocialProviderException()
    {
        var exception = new SocialProviderRateLimitException("Test", null, "X");

        exception.Should().BeAssignableTo<SocialProviderException>();
    }

    [Fact]
    public void SocialProviderAuthenticationException_InheritsFromSocialProviderException()
    {
        var exception = new SocialProviderAuthenticationException("Test", "X");

        exception.Should().BeAssignableTo<SocialProviderException>();
    }
}
