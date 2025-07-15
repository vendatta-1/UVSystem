using Dapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UVS.Common.Application.EventBus;
using UVS.Common.Infrastructure.Inbox;
using UVS.Common.Infrastructure.Serialization;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumer<TIntegrationEvent>
    (
        IDbConnectionFactory  dbConnectionFactory,
        ILogger<IntegrationEventConsumer<TIntegrationEvent>> logger)
:IConsumer<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{
    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    { 
        logger.LogInformation("{Module} - Consuming integration event: {IntegrationEventId}","System", context.Message.Id);
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();
        
        var integrationEvent = context.Message;

        var inboxMessage = new InboxMessage()
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
            OccurredOnUtc = integrationEvent.OccurredOnUtc,
        };
        var sql = $"""
                   INSERT INTO system.inbox_messages(id, type, content, occurred_on_utc)
                   VALUES(@Id, @Type, @Content::json, @OccurredOnUtc)";"
                   
                   """;
        await connection.ExecuteAsync(sql, inboxMessage);
    }
}
 