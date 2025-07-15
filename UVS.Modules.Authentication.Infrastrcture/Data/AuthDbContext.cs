using Microsoft.EntityFrameworkCore;
using UVS.Authentication.Domain.Users;
using UVS.Common.Infrastructure.Inbox;
using UVS.Common.Infrastructure.Outbox;
using UVS.Modules.Authentication.Application.Abstractions.Data;

namespace UVS.Authentication.Infrastructure.Data;

public sealed class AuthDbContext(DbContextOptions<AuthDbContext> options) :DbContext(options), IUnitOfWork
{
    public DbSet<User>  Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("auth");
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}