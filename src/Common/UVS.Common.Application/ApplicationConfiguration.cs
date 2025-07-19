using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UVS.Common.Application.Behaviors;

namespace UVS.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);

            config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
        foreach (var assembly in assemblies)
        {
            services.AddAutoMapper(assembly);
        }
        return services;
    }
}