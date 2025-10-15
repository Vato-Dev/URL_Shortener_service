using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.SqlEfCore.Configurations;

public class RegularUrlConfiguration : IEntityTypeConfiguration<RegularUrl>
{
    public void Configure(EntityTypeBuilder<RegularUrl> builder)
    {
        
        
    }
}