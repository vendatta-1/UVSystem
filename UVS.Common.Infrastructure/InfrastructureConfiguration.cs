using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;
using UVS.Common.Application.Caching;
using UVS.Common.Application.Clock;
using UVS.Common.Infrastructure.Caching;
using UVS.Common.Infrastructure.Clock;
using UVS.Common.Infrastructure.Data;
using UVS.Modules.System.Application.Data;
 
namespace UVS.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        NpgsqlDataSource dataSource = NpgsqlDataSource.Create(configuration.GetConnectionString("UVS")!);
        
        services.TryAddSingleton(dataSource);

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Cache")!);
            services.AddSingleton(connectionMultiplexer);
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch (Exception e)
        {
            //reason behind, for example, running migration when the docker image is off so instead of stop the other service use memory 
            Console.WriteLine(e);
            services.AddDistributedMemoryCache();

        }
        
        services.TryAddSingleton<ICacheService, CacheService>();
        return services;
    }
}