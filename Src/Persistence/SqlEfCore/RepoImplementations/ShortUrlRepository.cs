using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlEfCore.RepoImplementations;

public sealed class ShortUrlRepository(AppDbContext context) : BaseRepository<ShortUrl,Guid>(context), IShortUrlRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Guid> GetOriginalUrlIdByAliasOrCode(string aliasOrCode)
       => await _context.ShortUrls
            .Where(x => x.Alias == aliasOrCode || x.ShortUrlCode == aliasOrCode)
            .Select(x => x.RegularUrlId)
            .FirstOrDefaultAsync();
    
    
  
    public async Task<bool> IsAliasTaken(string alias, CancellationToken ct)
    {
        var normalizedAlias = alias.ToLowerInvariant();
      return await _context.ShortUrls.AnyAsync(x=>x.NormalizedAlias == normalizedAlias, ct);
    }

    public async Task<int> DeleteExpiredUrlsAsync(DateTime expiredFrom, CancellationToken ct)
    {
        var count = await _context.ShortUrls
            .Where(url => (url.LastClickedAt ?? url.CreatedAt) < expiredFrom)
            .ExecuteDeleteAsync(ct);
        return count;
    }

}