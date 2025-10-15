namespace Domain.Models;
    public sealed class RegularUrl
    {
        public Guid Id { get; init; }
        public string UrlString { get; set; }
        public DateTime CreatedAt { get; init; }

        public RegularUrl(string urlString)
        {
            Id = Guid.NewGuid();
            UrlString = urlString;
            CreatedAt = DateTime.UtcNow;
        }
    }