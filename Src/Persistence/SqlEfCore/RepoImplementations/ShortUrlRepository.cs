using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlEfCore.RepoImplementations;

public sealed class ShortUrlRepository(AppDbContext context) : BaseRepository<ShortUrl,Guid>(context), IShortUrlRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> IsAliasTaken(string alias, CancellationToken ct)
    {
        var normalizedAlias = alias.ToLowerInvariant();
      return await _context.ShortUrls.AnyAsync(x=>x.NormalizedAlias == normalizedAlias, ct);
    }

    public async Task<(int, List<Guid>)> DeleteExpiredUrlsAsync(DateTime expiredFrom, CancellationToken ct) // violation of single responsibility i just have no f* idea how to write otherwise
    {
        var id = await _context.ShortUrls
                 .Where(url => (url.LastClickedAt ?? url.CreatedAt) < expiredFrom)
        .Select(url => url.RegularUrlId)
        .Distinct()
        .ToListAsync(ct);
        
        var count = await _context.ShortUrls
            .Where(url => (url.LastClickedAt ?? url.CreatedAt) < expiredFrom)
            .ExecuteDeleteAsync(ct);
        return (count, id);
    }
    
}