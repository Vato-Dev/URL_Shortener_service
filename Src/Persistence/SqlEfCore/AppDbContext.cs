using System.Reflection;
using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlEfCore.ValueConverters;

namespace Persistence.SqlEfCore;

public sealed class AppDbContext(DbContextOptions options) : DbContext
{
    public DbSet<RegularUrl> RegularUrls { get; set; }
    public DbSet<ShortUrl> ShortUrls { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load(nameof(Persistence)));
        modelBuilder.ApplyUtcDateTimeConverter();
        base.OnModelCreating(modelBuilder);
    }
     
    
}
public static class ModelBuilderConvertersExtensions
{
    public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
    {
        var utcConverter = new DateTimeToUtcValueConverter();
        var nullableUtcConverter = new NullableDateTimeToUtcValueConverter();

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(utcConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableUtcConverter);
                }
            }
        }
    }
}