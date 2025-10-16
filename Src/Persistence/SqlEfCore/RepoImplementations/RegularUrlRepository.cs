using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SqlEfCore.RepoImplementations;

public class RegularUrlRepository(AppDbContext context) : BaseRepository<RegularUrl, Guid>(context), IRegularUrlRepository
{
    private readonly AppDbContext _context = context;

    public async Task<RegularUrl?> GetByUrlStringAsync(string url, CancellationToken ct)
    {
        var normalizedUrl = url.ToLowerInvariant();
        var result = await _context.RegularUrls
            .FirstOrDefaultAsync(x => x.NormalizedUrlString == normalizedUrl, ct);
        return result;
    }
}