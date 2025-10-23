using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlEfCore.RepoImplementations;

public sealed class RegularUrlRepository(AppDbContext context) : BaseRepository<RegularUrl, Guid>(context), IRegularUrlRepository
{
    private readonly AppDbContext _context = context;

    public async Task<RegularUrl?> GetByUrlStringAsync(string url, CancellationToken ct)
    {
        var normalizedUrl = url.ToLowerInvariant();
        var result = await _context.RegularUrls
            .FirstOrDefaultAsync(x => x.NormalizedUrlString == normalizedUrl, ct);
        return result;
    }

    public async Task<int> DeleteAllOrphanUrls(List<Guid> ids, CancellationToken ct)
    {
      var result =  await _context.RegularUrls
            .Where(r => ids.Contains(r.Id))
            .Where(r => !_context.ShortUrls.Any(s => s.RegularUrlId == r.Id))
            .ExecuteDeleteAsync(ct);
      return result;
    }
}