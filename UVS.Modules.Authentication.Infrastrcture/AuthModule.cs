using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using UVS.Common.Application.EventBus;
using UVS.Common.Application.Messaging;
using UVS.Common.Infrastructure.Outbox;
using UVS.Modules.Authentication.Application.Abstractions.Data;
using UVS.Modules.Authentication.Application.Abstractions.Identity;
using UVS.Modules.Authentication.Application.Service;
using UVS.Modules.Authentication.Domain.Users;
using UVS.Modules.Authentication.Infrastructure.Data;
using UVS.Modules.Authentication.Infrastructure.Identity;
using UVS.Modules.Authentication.Infrastructure.Inbox;
using UVS.Modules.Authentication.Infrastructure.Outbox;
using UVS.Modules.Authentication.Infrastructure.Repositories;
using UVS.Modules.Authentication.Infrastructure.Services;
using UVS.Modules.System.Integration;

namespace UVS.Modules.Authentication.Infrastructure;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration)
            .AddServices()
            .AddIntegrationEventHandlers()
            .AddDomainEventHandlers();
            
        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<CreateStudentIntegrationEvent>>();
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

        services.AddTransient<KeyCloakAuthDelegatingHandler>();



        services
            .AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                KeyCloakOptions keycloakOptions = serviceProvider
                    .GetRequiredService<IOptions<KeyCloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        services.AddTransient<IIdentityProviderService, IdentityProviderService>();

        services.AddDbContext<AuthDbContext>((sp, opt) =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("UVS"),
                npOpt =>
                {
                    npOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                    npOpt.MigrationsHistoryTable("Auth_Migrations", "auth");
                }).UseSnakeCaseNamingConvention();
            opt.AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>());
            opt.EnableSensitiveDataLogging();
            opt.EnableDetailedErrors();
        });

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AuthDbContext>());

        services.Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"));

        services.ConfigureOptions<ConfigureOutboxProcessJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Users:Inbox"));

        services.ConfigureOptions<ConfigureInboxProcessJob>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IPasswordService, PasswordService>();
        return services;
    }
    private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }

        return services;
    }

    private static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }

        return services;
    }
}