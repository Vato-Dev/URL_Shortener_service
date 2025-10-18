using Application;
using Application.Abstractions;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.SqlEfCore.RepoImplementations;

namespace Persistence.SqlEfCore;

public class ConfigurePersistence : ConfigurationBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            if (IsDevelopment)
            {
                builder.EnableDetailedErrors();
                builder.EnableSensitiveDataLogging();
            }

            var dbFilePath = "DB__PATH".FromEnvRequired();  
            var dbDirectory = Path.GetDirectoryName(dbFilePath)!;

            var directory = new DirectoryInfo(dbDirectory);
            if (!directory.Exists)
                directory.Create();

            builder.UseSqlite(
                $"Data Source={dbFilePath}",
                sqliteOptions => sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            
        });
        
        
        services.AddScoped<IRegularUrlRepository, RegularUrlRepository>();
        services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
        //services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
    }

}