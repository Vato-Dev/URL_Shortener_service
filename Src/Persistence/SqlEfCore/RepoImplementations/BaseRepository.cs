using Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlEfCore.RepoImplementations;

public abstract class BaseRepository<TEntity, TId>(AppDbContext context) : IRepository<TEntity, TId>
    where TEntity : class
{
    public async Task CreateAsync(TEntity entity, CancellationToken ct)
    {
        context.Set<TEntity>().Add(entity);
       await  context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
         context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(TEntity entity, CancellationToken ct)
    {
        context.Set<TEntity>().Remove(entity);
       await context.SaveChangesAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct) =>
       await context.Set<TEntity>().FindAsync(id,ct).AsTask();
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct) => await context.Set<TEntity>().AsNoTracking().ToListAsync(ct);// bit risky AsNoTraking 
}