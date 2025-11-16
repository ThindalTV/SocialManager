using SocialManager.Data.Types.Social;

namespace SocialManager.SocialProvider.X;

public interface ISocialProvider
{
    Task Post(SocialPost post, CancellationToken ct);
}
