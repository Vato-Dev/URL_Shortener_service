using Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlEfCore.RepoImplementations;

public abstract class BaseRepository<TEntity, TId>(AppDbContext context) : IRepository<TEntity, TId>
    where TEntity : class
{
    public Task CreateAsync(TEntity entity, CancellationToken ct)
    {
        context.Set<TEntity>().Add(entity);
        context.SaveChangesAsync(ct);
        return Task.CompletedTask;
    }
    public Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
         context.Set<TEntity>().Update(entity);
         context.SaveChangesAsync(ct);
        return Task.CompletedTask;
    }
    public Task DeleteAsync(TEntity entity, CancellationToken ct)
    {
        context.Set<TEntity>().Remove(entity);
        context.SaveChangesAsync(ct);
        return Task.CompletedTask;
    }
    
    public Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct) => context.Set<TEntity>().FindAsync([id!], ct).AsTask();
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct) => await context.Set<TEntity>().AsNoTracking().ToListAsync(ct);// bit risky AsNoTraking 
}