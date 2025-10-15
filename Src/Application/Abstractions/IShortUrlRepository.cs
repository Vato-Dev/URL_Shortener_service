using Domain;

namespace Application.Abstractions;

public interface IShortUrlRepository :  IRepository<ShortUrl,Guid>
{
    Task<bool> ExistsByAliasAsync(string alias, CancellationToken ct); 
}