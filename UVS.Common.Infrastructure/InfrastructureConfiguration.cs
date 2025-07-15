using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Quartz;
using StackExchange.Redis;
using UVS.Common.Application.Caching;
using UVS.Common.Application.Clock;
using UVS.Common.Application.EventBus;
using UVS.Common.Infrastructure.Authentication;
using UVS.Common.Infrastructure.Caching;
using UVS.Common.Infrastructure.Clock;
using UVS.Common.Infrastructure.Data;
using UVS.Common.Infrastructure.Outbox;
using UVS.Modules.System.Application.Data;

namespace UVS.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        Action<IRegistrationConfigurator>[] moduleConfigureConsumers)
    {
        NpgsqlDataSource dataSource = NpgsqlDataSource.Create(configuration.GetConnectionString("UVS")!);

        services.TryAddSingleton(dataSource);

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();
        
        services.AddAuthenticationInternal();

        services.AddQuartz();
        
        services.AddQuartzHostedService(x=>x.WaitForJobsToComplete=true);
        
        try
        {
            IConnectionMultiplexer connectionMultiplexer =
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("Cache")!);
            services.AddSingleton(connectionMultiplexer);
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch (Exception e)
        {
            //reason behind, for example, running migration when the docker image is off, so instead of stop the other service use memory 

            services.AddDistributedMemoryCache();
        }

        services.TryAddSingleton<ICacheService, CacheService>();

        services.TryAddSingleton<IEventBus, EventBus.EventBus>();

        services.AddMassTransit(config =>
        {
            foreach (var moduleConfigureConsumer in moduleConfigureConsumers)
            {
                moduleConfigureConsumer(config);
            }

            config.SetKebabCaseEndpointNameFormatter();
            config.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
        });


        return services;
    }
}