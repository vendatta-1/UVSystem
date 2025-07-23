using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UVS.Common.Application.EventBus;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain; 
using UVS.Common.Infrastructure.Outbox;
using UVS.Modules.System.Application.Data;
using UVS.Domain.Courses;
using UVS.Domain.Departments;
using UVS.Domain.Instructors;
using UVS.Domain.Schedules;
using UVS.Domain.Semesters;
using UVS.Domain.Students; 
using UVS.Modules.System.Infrastructure.Data;
using UVS.Modules.System.Infrastructure.Inbox;
using UVS.Modules.System.Infrastructure.Outbox;
using UVS.Modules.System.Infrastructure.Repositories; 

namespace UVS.Modules.System.Infrastructure;

public static class SystemModule
{
    public static IServiceCollection AddSystemModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers()
            .AddIntegrationEventHandlers()
            .AddServices()
            .AddInfrastructure(configuration);
        
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UVSDbContext>((sp,opt) =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("UVS"),
                npOpt =>
                {
                    npOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                    npOpt.MigrationsHistoryTable("UVS_Migrations", "uvs");
                }).UseSnakeCaseNamingConvention();
            opt.AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
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

        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        
        
        
        services.Configure<OutboxOptions>(configuration.GetSection("System:Outbox"));
        services.ConfigureOptions<ConfigureOutboxProcessJob>();
        
        services.Configure<InboxOptions>(configuration.GetSection("System:Inbox"));
        
        
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    { 
        return services;
    }
    private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services )
    {

         var handlers = Application.AssemblyReference.Assembly
             .GetTypes()
             .Where(t=>t.IsAssignableTo(typeof(IDomainEventHandler)))
             .ToList();

         foreach (var domainHandler in handlers)
         {

             services.TryAddScoped(domainHandler);
             //next decorate using Idempotent decorator
             //first extract domain event 
             var domainEvent = domainHandler
                 .GetInterfaces()
                 .Single(i => i.IsGenericType)
                 .GetGenericArguments()
                 .Single();

             Type closedIdempotentType = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

             services.Decorate(domainHandler, closedIdempotentType);

         }        
         
         return services;
         
    }

    private static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
    {
        var handlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler<>)))
            .ToList();

        foreach (var handler in handlers)
        {
            var integrationEvent = handler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();
            
            Type closedIdempotentType = typeof(IdempotentDomainEventHandler<>).MakeGenericType(integrationEvent);
            services.Decorate(handler, closedIdempotentType);
        }

        return services;

    }
}