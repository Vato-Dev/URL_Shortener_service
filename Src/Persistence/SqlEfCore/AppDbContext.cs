using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlEfCore.ValueConverters;

namespace Persistence.SqlEfCore;

public sealed class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<RegularUrl> RegularUrls { get; set; }
    public DbSet<ShortUrl> ShortUrls { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load(nameof(Persistence)));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyUtcDateTimeConverter();
       // modelBuilder.ApplyGuidToByteArrayConverter();
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

  /*  public static void ApplyGuidToByteArrayConverter(this ModelBuilder builder)
    {
        var toTyteConventer = new GuidToByteValueConverter();
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(Guid))
                {
                    property.SetValueConverter(toTyteConventer);
                }
            }
        }
    }*/
}
/*
//FOR DEVELOP
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var dbDirectory = Path.GetDirectoryName("G:/C#projects/PRACTICE/URL_Shortener_service/.data/app.db")!;

        var directory = new DirectoryInfo(dbDirectory);
        if (!directory.Exists)
            directory.Create();
        optionsBuilder.UseSqlite("Data Source=G:/C#projects/PRACTICE/URL_Shortener_service/.data/app.db",
            sqliteOptions => sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

        return new AppDbContext(optionsBuilder.Options);
    }
}*/