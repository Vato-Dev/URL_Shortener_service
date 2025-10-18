using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.SqlEfCore.Configurations;

public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(s => s.RegularUrlId).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.LastClickedAt);
        builder.Property(s => s.ClickCount).IsRequired();

        builder.Property(s => s.Alias)
            .HasMaxLength(50);
        builder.Property(s => s.NormalizedAlias)
            .HasMaxLength(50);
        builder.Ignore(x => x.HasAlias);

        builder.Property(x=>x.ShortUrlCode)
            .HasConversion(
                urlCode=>urlCode.Value ,
                urlCode=>UrlCode.Create(urlCode));
        
        
        builder.HasIndex(i => i.NormalizedAlias)
            .IsUnique()
            .HasFilter("\"NormalizedAlias\" IS NOT NULL");
        
        builder.HasOne<RegularUrl>()
            .WithMany()
            .HasForeignKey(a=>a.RegularUrlId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}