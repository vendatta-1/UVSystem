using System.Data;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using UVS.Common.Application.Clock;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Common.Infrastructure.Outbox;
using UVS.Common.Infrastructure.Serialization;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.Authentication.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxJob (
    IDateTimeProvider dateTimeProvider,
    IServiceScopeFactory scopeFactory,
    IDbConnectionFactory dbConnectionFactory,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger)
: IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        
        /* operations flow
         * 1- open connection and transaction
         * 2- grep messages from db based on (schema and processed_on_utc == null)
         * 3- for each message deserialize content and then grep handlers from DomainEventHandlersFactory
         * 4- fire
         * 5- update and catch errors of the operation
         */
        logger.LogInformation("{Module}- Executing outbox job", "Authentication");
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        await using var transaction = await dbConnection.BeginTransactionAsync();
        IReadOnlyCollection<OutboxMessageResponse> messages = await GetOutboxMessagesAsync(dbConnection, transaction);

        foreach (OutboxMessageResponse outboxMessage in messages)
        {
            Exception? exception = null;
            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    SerializerSettings.Instance)!;

                using IServiceScope scope = scopeFactory.CreateScope();

                IEnumerable<IDomainEventHandler> domainEventHandlers = DomainEventHandlersFactory.GetHandlers(
                    domainEvent.GetType(),
                    scope.ServiceProvider,
                    Modules.Authentication.Application.AssemblyReference.Assembly);

                foreach (IDomainEventHandler domainEventHandler in domainEventHandlers)
                {
                    await domainEventHandler.Handle(domainEvent);
                }
            }
            catch (Exception caughtException)
            {
                logger.LogError(
                    caughtException,
                    "{Module} - Exception while processing outbox message {MessageId}",
                    "System",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(dbConnection, transaction, outboxMessage, exception);
            
        }
        await transaction.CommitAsync();
    }

    private async Task UpdateOutboxMessageAsync(IDbConnection connection, IDbTransaction transaction,
        OutboxMessageResponse outboxMessage, Exception? exception = null)
    {
        string sql =
            $"""
                UPDATE auth.outbox_messages
                SET processed_on_utc = @ProcessedOnUtc,
                    error = @Error
                 WHERE id = @Id
             """;
        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: transaction);
    }
    private async  Task<IReadOnlyCollection<OutboxMessageResponse>> GetOutboxMessagesAsync(IDbConnection dbConnection,
        IDbTransaction transaction)
    {
        string sql =
            $"""
             SELECT
                id AS {nameof(OutboxMessageResponse.Id)},
                content AS {nameof(OutboxMessageResponse.Content)}
             FROM auth.outbox_messages
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {outboxOptions.Value.BatchSize}
             FOR UPDATE
             """;
        var messages = await dbConnection.QueryAsync< OutboxMessageResponse>(
            sql,
            transaction: transaction);

        return messages.ToList();

    }
}