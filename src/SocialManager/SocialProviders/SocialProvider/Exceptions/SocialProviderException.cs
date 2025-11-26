namespace SocialProvider.Exceptions;

/// <summary>
/// Base exception for social provider errors.
/// </summary>
public class SocialProviderException : Exception
{
    /// <summary>
    /// Gets the platform identifier where the error occurred.
    /// </summary>
    public string? Platform { get; }

    public SocialProviderException(string message, string? platform = null) : base(message)
    {
        Platform = platform;
    }

    public SocialProviderException(string message, Exception innerException, string? platform = null) 
        : base(message, innerException)
    {
        Platform = platform;
    }
}
