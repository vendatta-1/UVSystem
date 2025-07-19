using UVS.Common.Infrastructure.Inbox;
using UVS.Common.Infrastructure.Outbox;
using UVS.Modules.System.Application.Data;
using UVS.Domain.Courses;
using UVS.Domain.Departments;
using UVS.Domain.Enrollments;
using UVS.Domain.Instructors;
using UVS.Domain.Schedules;
using UVS.Domain.Semesters;
using UVS.Domain.Students;

namespace UVS.Modules.System.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public sealed class UVSDbContext(DbContextOptions<UVSDbContext> options) : DbContext(options),IUnitOfWork
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.HasDefaultSchema("system");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UVSDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    }
}