namespace Domain.Models;
    public sealed class RegularUrl(string urlString) // maybe should be a value object ,but because of lack logic rn i can't decide
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string UrlString { get; init; } = urlString;
        public string NormalizedUrlString => UrlString.ToLowerInvariant();
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }