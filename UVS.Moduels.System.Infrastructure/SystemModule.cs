
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using UVS.Common.Infrastructure.Data;
using UVS.Modules.System.Application.Data;
using UVS.Domain.Courses;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Domain.Repository;
using UVS.Domain.Semesters;
using UVS.Domain.Students;
using UVS.Modules.System.Infrastructure.Data;
using UVS.Modules.System.Infrastructure.Repositories;

namespace UVS.Modules.System.Infrastructure;

public static class SystemModule{
    
    public static IServiceCollection AddSystemModule(this IServiceCollection services, IConfiguration configuration)
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
   
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UVSDbContext>());
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IStudentRepository, StudentRepository>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        
        services.AddScoped<IInstructorRepository, InstructorRepository>();

        services.AddScoped<ISemesterRepository, SemesterRepository>();
        
        return services;
    }
}