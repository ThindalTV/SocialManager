using SocialManager.Data.Repositories;

namespace SocialManager.Data;

public interface IUnitOfWork
{
    Task AttachRepository(IRepository repository, CancellationToken ct);

    Task StartTransaction(CancellationToken ct);

    Task SaveChanges(CancellationToken ct); // Saves changes without committing the transaction

    Task CommitTransaction(CancellationToken ct);
    Task RollbackTransaction(CancellationToken ct);
}
