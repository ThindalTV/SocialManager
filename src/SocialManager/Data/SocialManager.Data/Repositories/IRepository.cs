using SocialManager.Data.Types;

namespace SocialManager.Data.Repositories;

public interface IRepository<T> : IRepository where T : BaseType
{
    Task<T?> GetById(string id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> Get(int page, int pageSize, CancellationToken ct);
    
    Task Add(T obj, CancellationToken ct);
    Task Update(T obj, CancellationToken ct);
    Task Delete(T obj, CancellationToken ct);

    Task SyncChanges(CancellationToken ct);
}

public interface IRepository
{
}
