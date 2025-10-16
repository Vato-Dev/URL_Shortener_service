using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlEfCore.RepoImplementations;

public class ShortUrlRepository(AppDbContext context) : BaseRepository<ShortUrl,Guid>(context), IShortUrlRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> IsAliasTaken(string alias, CancellationToken ct)
    {
        var normalizedAlias = alias.ToLowerInvariant();
      return await _context.ShortUrls.AnyAsync(x=>x.NormalizedAlias == normalizedAlias, ct);
    }
}