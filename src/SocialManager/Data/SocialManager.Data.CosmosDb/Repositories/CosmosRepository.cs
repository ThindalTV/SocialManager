using Microsoft.EntityFrameworkCore;
using SocialManager.Data.Repositories;
using SocialManager.Data.Types;
using System.Linq.Expressions;

namespace SocialManager.Data.CosmosDb.Repositories;

public class CosmosRepository<T> : IRepository<T> where T : BaseType
{
    private readonly SocialManagerDbContext _context;
    protected readonly IQueryable<T> Set;

    public CosmosRepository(SocialManagerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Set = _context.Set<T>().AsNoTracking();
    }

    public async Task<T?> GetById(string id, CancellationToken cancellationToken)
    {
        return await GetById(id, null, cancellationToken);
    }

    public virtual async Task<T?> GetById(string id, bool? isPublished, CancellationToken cancellationToken)
    {
        var query = Set.Where(e => e.Id == id && !e.IsDeleted);

        if (isPublished.HasValue)
        {
            query = query.Where(e => e.IsPublished == isPublished.Value);
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> Get(int page, int pageSize, CancellationToken ct)
    {
        return await Get(page, pageSize, null, ct);
    }

    public virtual async Task<IEnumerable<T>> Get(int page, int pageSize, bool? isPublished, CancellationToken ct)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0");
        
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be greater than 0");

        var query = Set.Where(e => !e.IsDeleted);

        if (isPublished.HasValue)
        {
            query = query.Where(e => e.IsPublished == isPublished.Value);
        }

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public virtual async Task Add(T obj, CancellationToken ct)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        obj.CreatedDate = DateTimeOffset.UtcNow;
        obj.UpdatedDate = DateTimeOffset.UtcNow;

        // Attach and mark as Added
        var entry = _context.Entry(obj);
        if (entry.State == EntityState.Detached)
        {
            await _context.Set<T>().AddAsync(obj, ct);
        }
        else
        {
            entry.State = EntityState.Added;
        }
    }

    public virtual Task Update(T obj, CancellationToken ct)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        obj.UpdatedDate = DateTimeOffset.UtcNow;

        // Attach and mark as Modified
        var entry = _context.Entry(obj);
        if (entry.State == EntityState.Detached)
        {
            _context.Set<T>().Attach(obj);
            entry.State = EntityState.Modified;
        }
        else
        {
            entry.State = EntityState.Modified;
        }

        return Task.CompletedTask;
    }

    public virtual Task Delete(T obj, CancellationToken ct)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        // Soft delete
        obj.IsDeleted = true;
        obj.UpdatedDate = DateTimeOffset.UtcNow;

        // Attach and mark as Modified
        var entry = _context.Entry(obj);
        if (entry.State == EntityState.Detached)
        {
            _context.Set<T>().Attach(obj);
            entry.State = EntityState.Modified;
        }
        else
        {
            entry.State = EntityState.Modified;
        }

        return Task.CompletedTask;
    }

    public virtual async Task SyncChanges(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}
