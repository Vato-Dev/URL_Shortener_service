using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.SqlEfCore.Configurations;

public class RegularUrlConfiguration : IEntityTypeConfiguration<RegularUrl>
{
    public void Configure(EntityTypeBuilder<RegularUrl> builder)
    {
        builder.HasKey(x=>x.Id);
        builder.Property(x=>x.UrlString).IsRequired().HasMaxLength(2048);
    
        builder.Property(x=>x.NormalizedUrlString).HasMaxLength(2048).IsRequired();
        builder.HasIndex(x=>x.NormalizedUrlString).IsUnique();
    }
}