namespace Domain.Models;
    public sealed class RegularUrl // maybe should be a value object ,but because of lack logic rn i can't decide
    {
        public RegularUrl(string urlString)
        {
            UrlString = urlString;
            NormalizedUrlString = urlString.ToLowerInvariant();
        }

        public Guid Id { get; init; } = Guid.NewGuid();
        public string UrlString { get; init; }
        public string NormalizedUrlString {get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

      
        
        
    }