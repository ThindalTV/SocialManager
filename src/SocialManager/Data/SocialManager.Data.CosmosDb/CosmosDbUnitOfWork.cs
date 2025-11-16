using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SocialManager.Data.Repositories;
using SocialManager.Data;

namespace SocialManager.Data.CosmosDb;

public class CosmosDbUnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private readonly SocialManagerDbContext _context;
    private IDbContextTransaction? _transaction;
    private readonly List<IRepository> _repositories = [];
    private bool _disposed;

    public CosmosDbUnitOfWork(SocialManagerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task AttachRepository(IRepository repository, CancellationToken ct)
    {
        if (repository == null)
            throw new ArgumentNullException(nameof(repository));

        if (!_repositories.Contains(repository))
        {
            _repositories.Add(repository);
        }

        return Task.CompletedTask;
    }

    public async Task StartTransaction(CancellationToken ct)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task SaveChanges(CancellationToken ct)
    {
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            // Log or handle the exception as needed
            throw new InvalidOperationException("An error occurred while saving changes to the database.", ex);
        }
    }

    public async Task CommitTransaction(CancellationToken ct)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }

        try
        {
            await _context.SaveChangesAsync(ct);
            await _transaction.CommitAsync(ct);
        }
        catch
        {
            await RollbackTransaction(ct);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransaction(CancellationToken ct)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction is in progress.");
        }

        try
        {
            await _transaction.RollbackAsync(ct);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
        _disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }

            await _context.DisposeAsync();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}
