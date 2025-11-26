namespace SocialProvider.Exceptions;

/// <summary>
/// Exception thrown when social provider API authentication fails.
/// </summary>
public class SocialProviderAuthenticationException : SocialProviderException
{
    public SocialProviderAuthenticationException(string message, string? platform = null) 
        : base(message, platform)
    {
    }

    public SocialProviderAuthenticationException(string message, Exception innerException, string? platform = null) 
        : base(message, innerException, platform)
    {
    }
}
