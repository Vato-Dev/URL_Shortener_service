namespace Application.Abstractions;

public interface IRepository<TEntity, in TId> where TEntity : class
{
    Task CreateAsync(TEntity entity,CancellationToken ct);
    Task UpdateAsync(TEntity entity,CancellationToken ct);
    Task DeleteAsync(TEntity entity,CancellationToken ct);
    
    Task<TEntity?> GetByIdAsync(TId id,CancellationToken ct);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
}