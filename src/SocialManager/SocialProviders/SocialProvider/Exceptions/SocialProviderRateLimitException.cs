namespace SocialProvider.Exceptions;

/// <summary>
/// Exception thrown when a social provider API rate limit is exceeded.
/// </summary>
public class SocialProviderRateLimitException : SocialProviderException
{
    /// <summary>
    /// Gets the time when the rate limit will reset.
    /// </summary>
    public DateTime? ResetTime { get; }

    public SocialProviderRateLimitException(string message, DateTime? resetTime = null, string? platform = null) 
        : base(message, platform)
    {
        ResetTime = resetTime;
    }

    public SocialProviderRateLimitException(string message, DateTime? resetTime, Exception innerException, string? platform = null) 
        : base(message, innerException, platform)
    {
        ResetTime = resetTime;
    }
}
