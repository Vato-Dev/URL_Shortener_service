using System.Linq.Expressions;
using Domain;
using Domain.Models;

namespace Application.Abstractions;

public interface IShortUrlRepository :  IRepository<ShortUrl,Guid>
{
    Task<bool> IsAliasTaken(string alias, CancellationToken ct);
    Task<(int,List<Guid>)> DeleteExpiredUrlsAsync(DateTime expiredFrom ,CancellationToken ct);
}