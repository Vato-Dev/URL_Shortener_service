using Domain.ValueObjects;

namespace Domain.Models;

public sealed class ShortUrl
{
    private DateTime? _lastClickedAt;
    public Guid Id { get; init; }
    public Guid RegularUrlId { get; init; }
    public UrlCode ShortUrlCode { get; private set; } = null!;
    public DateTime CreatedAt {get; init; }
    public DateTime? LastClickedAt { get; private set; }
    public string? Alias { get; init; }
    public string? NormalizedAlias { get; private set; }
    public bool HasAlias => !string.IsNullOrEmpty(Alias);
    public long ClickCount { get;private  set; } //make a logic with redis + flush job (every 15 minutes update db , not critical)

    private ShortUrl()
    {
    }

    public static ShortUrl Create(string? alias,Guid originalUrlId) // add logic if alias is given
    {
        var url = new ShortUrl
        {
            Id = Guid.NewGuid(),
            RegularUrlId = originalUrlId,
            ShortUrlCode = UrlCode.GenerateCode(),
            CreatedAt = DateTime.UtcNow,
            LastClickedAt = null,
            Alias = alias,
            NormalizedAlias = alias?.ToLowerInvariant()
        };
        return url;
    }

    public bool IsExpired(DateTime currentDateTime)
    {
        var lastActive = LastClickedAt ?? CreatedAt;
        return (currentDateTime - lastActive).TotalDays > 30;
    }
    public void Click()
    {
        ClickCount++;
        _lastClickedAt = DateTime.UtcNow;
    }
}