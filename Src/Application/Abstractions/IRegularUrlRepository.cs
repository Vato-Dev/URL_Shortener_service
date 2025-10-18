using Domain.Models;

namespace Application.Abstractions;

public interface IRegularUrlRepository : IRepository<RegularUrl,Guid>
{ 
    Task<RegularUrl?> GetByUrlStringAsync(string url, CancellationToken ct);
}