using Domain.Models;

namespace Domain.Tests.Models;

public class ShortUrlTests
{
    
    [Theory(DisplayName = "IsExpired checking")]
    [InlineData(30, 0, false, "is active 30 days")]
    [InlineData(30, 1, true, "Expired for 1 second")]
    [InlineData(31, 0, true, "Expired for Month")]
    public void IsExpired_Should_Correctly_Determine_Expiration(int daysPassed, int secondsPassed, bool expected, string description)
    {
        // Arrange
        var originalUrlId = Guid.NewGuid();
        var shortUrl = ShortUrl.Create(null, originalUrlId);
        var createdTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    
        // Act
        var currentTestTime = createdTime
            .AddDays(daysPassed)
            .AddSeconds(secondsPassed);
    
        var isExpired = shortUrl.IsExpired(currentTestTime);

        // Assert
        Assert.Equal(expected, isExpired);
    }

}