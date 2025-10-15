using System.Runtime.ConstrainedExecution;
using Domain.ValueObjects;

namespace Domain;

public sealed class ShortUrl
{
    private DateTime? _LastClickedAt;
    public Guid Id { get; init; }
    public Guid OriginalUrlId { get; init; }
    public UrlCode ShortUrlCode { get; private set; }
    public DateTime CreatedAt {get; init; }
    public DateTime? LastClickedAt
    {
        get => _LastClickedAt;
        private set => _LastClickedAt = value ?? DateTime.UtcNow.AddDays(30);
    }
    private string? Alias { get; init; }
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
            OriginalUrlId = originalUrlId,
            ShortUrlCode = UrlCode.GenerateCode(),
            CreatedAt = DateTime.UtcNow,
            LastClickedAt = DateTime.UtcNow.AddDays(30),
            Alias = alias,
            ClickCount = 0
        };
        return url;
    }
    //public bool Update i'll make background job 

    public bool IsExpired()
    {
        var lastActive = LastClickedAt ?? CreatedAt;
        return (DateTime.UtcNow - lastActive).TotalDays > 30;
    }
   
}