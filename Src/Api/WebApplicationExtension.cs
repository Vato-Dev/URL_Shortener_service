using Microsoft.EntityFrameworkCore;
using Persistence.SqlEfCore;
using Serilog;

namespace Api;

public static class WebApplicationExtension
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            var migrations = await dbContext.Database.GetPendingMigrationsAsync();
            Log.Information("Applying migrations: {Migrations}", string.Join(", ", migrations));
            await dbContext.Database.MigrateAsync();
        }

        Log.Information("All migrations applied");

        await dbContext.SaveChangesAsync();
    }
}