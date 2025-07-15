using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using UVS.Authentication.Domain.Users;
using UVS.Authentication.Infrastructure.Data;
using UVS.Authentication.Infrastructure.Identity;
using UVS.Authentication.Infrastructure.Inbox;
using UVS.Authentication.Infrastructure.Outbox;
using UVS.Authentication.Infrastructure.Repositories;
using UVS.Authentication.Infrastructure.Services;
using UVS.Common.Infrastructure.Outbox;
using UVS.Modules.Authentication.Application.Abstractions.Data;
using UVS.Modules.Authentication.Application.Abstractions.Identity;
using UVS.Modules.Authentication.Application.Service;
using UVS.Modules.System.Integration;

namespace UVS.Authentication.Infrastructure;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration)
            .AddServices()
            ;
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
}