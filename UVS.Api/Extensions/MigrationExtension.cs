using Microsoft.EntityFrameworkCore;
using UVS.Modules.System.Infrastructure.Data;

namespace UVS.Api.Extensions;

internal static class MigrationExtension
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        ApplyMigration<UVSDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
    where TDbContext : DbContext
    {
        var dbContext=  scope.ServiceProvider.GetRequiredService<TDbContext>();
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
            dbContext.Database.Migrate();
        
    }
}