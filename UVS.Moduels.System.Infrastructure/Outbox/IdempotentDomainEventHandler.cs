using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Extensions.Logging;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Common.Infrastructure.Outbox;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.System.Infrastructure.Outbox;

/// <summary>
/// For adding idempotency to ensure that only one consumer is called only once and there is non duplication or do the same action twice
/// </summary>
internal sealed class IdempotentDomainEventHandler<TDomainEvent>
(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<IdempotentDomainEventHandler<TDomainEvent>> logger,
    IDomainEventHandler<TDomainEvent> decorated): 
    DomainEventHandler<TDomainEvent>
where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.Id, decorated.GetType().Name); //here name of event handler not the event itself
        if (await OutboxConsumerExistsAsync(dbConnection, outboxMessageConsumer))
        {
            return;
        }
        //case when not exists add it
        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(dbConnection, outboxMessageConsumer);
        
    }

    private async Task<bool> OutboxConsumerExistsAsync(IDbConnection connection, OutboxMessageConsumer outboxMessageConsumer)
    {
        string sql = $"""
                      SELECT EXISTS
                      (
                        SELECT 1
                        FROM system.outbox_message_consumers as omc
                        where omc.outbox_message_id = @OutboxMessageId AND omc.name =@Name
                      ) 
                      """;
        
        return await connection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
    }

    private static async Task InsertOutboxConsumerAsync(
        DbConnection dbConnection,
        OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            """
            INSERT INTO system.outbox_message_consumers(outbox_message_id, name)
            VALUES (@OutboxMessageId, @Name)
            """;

        await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}