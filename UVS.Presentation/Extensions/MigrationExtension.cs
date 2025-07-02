using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using UVS.Infrastructure.Data;

namespace UVS.Presentation.Extensions;

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