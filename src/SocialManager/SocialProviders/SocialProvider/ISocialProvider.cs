using SocialManager.Data.Types.Social;

namespace SocialProvider;

public interface ISocialProvider
{
    Task Post(Post post, CancellationToken ct);

    Task<Statistic> GetStatistics(string postId, CancellationToken ct);
}
