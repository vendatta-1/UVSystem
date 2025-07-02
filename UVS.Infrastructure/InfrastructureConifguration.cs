using Evently.Common.Infrastructure.Clock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using UVS.Application.Clock;
using UVS.Application.Data;
using UVS.Domain.Courses;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Domain.Repository;
using UVS.Domain.Students;
using UVS.Infrastructure.Data;
using UVS.Infrastructure.Repositories;

namespace UVS.Infrastructure;

public static class InfrastructureConifguration{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<UVSDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("UVS"),
            npOpt =>
            {
                npOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                npOpt.MigrationsHistoryTable("UVS_Migrations","UVS");
                
            }).
                UseSnakeCaseNamingConvention();
            opt.EnableSensitiveDataLogging();
            opt.EnableDetailedErrors();
            
        });
        NpgsqlDataSource dataSource = NpgsqlDataSource.Create(configuration.GetConnectionString("UVS")!);
        
        services.TryAddSingleton(dataSource);
        
        services.AddScoped<IDateTimeProvider,DateTimeProvider>();

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UVSDbContext>());
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IStudentRepository, StudentRepository>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();
        return services;
    }
}