namespace SocialManager.SocialProvider.X.Client.Models;

/// <summary>
/// Represents authenticated user information from the X API.
/// </summary>
public class XAuthenticatedUserResponse
{
    /// <summary>
    /// Gets or sets the user's unique identifier.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the user's username (without @).
    /// </summary>
    public required string Username { get; set; }
}
