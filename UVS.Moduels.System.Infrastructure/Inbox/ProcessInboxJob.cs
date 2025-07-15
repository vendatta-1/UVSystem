using System.Data; 
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using UVS.Common.Application.Clock;
using UVS.Common.Application.EventBus;
using UVS.Common.Infrastructure.Inbox;
using UVS.Common.Infrastructure.Serialization;
using UVS.Modules.System.Application.Data;
using UVS.Modules.System.Presentation;
namespace UVS.Modules.System.Infrastructure.Inbox;




[DisallowConcurrentExecution]
public sealed class ProcessInboxJob (
    IDateTimeProvider  dateTimeProvider, 
    IServiceScopeFactory scopeFactory,
    IDbConnectionFactory connection,
    IOptions<InboxOptions> options,
    ILogger<ProcessInboxJob> logger): IJob
{ 
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{Module} - Inbox job execution started","System");
        await using var dbConnection = await connection.OpenConnectionAsync();
        await using var transaction = await dbConnection.BeginTransactionAsync();
        var messages = await GetInboxMessages(dbConnection, transaction);

        foreach (var message in messages)
        {
            Exception? exception = null;
            using var scope = scopeFactory.CreateScope();
            try
            {
                var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(
                    message.Content,
                    SerializerSettings.Instance)!;
                
                var handlers = IntegrationEventHandlersFactory.GetHandlers(integrationEvent.GetType(),
                    scope.ServiceProvider,
                Presentation.AssemblyReference.Assembly);
                
                foreach (var handler in handlers)
                {
                    await handler.Handle(integrationEvent);
                }
                
                
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
                logger.LogError(
                    caughtException,
                    "{Module} - Exception while processing outbox message {MessageId}",
                    "System",
                    message.Id);
            }
            await UpdateMessageAsync(message, exception,transaction, dbConnection);
        }

        await transaction.CommitAsync();
    }

    private   async Task<IReadOnlyCollection<InboxMessageResponse>> GetInboxMessages(IDbConnection dbConnection,
        IDbTransaction transaction)
    {
        var sql = $"""
                   SELECT 
                        id as {nameof(InboxMessageResponse.Id)}
                        content as {nameof(InboxMessageResponse.Content)}
                        FROM system.inbox_messages
                        WHERE 
                            WHERE processed_on_utc IS NULL
                            ORDER BY occurred_on_utc
                        LIMIT {options.Value.BatchSize}
                        FOR UPDATE
                   """;
        var response =await dbConnection.QueryAsync<InboxMessageResponse>(sql,transaction);
        return response.ToList();
    }

    private async Task UpdateMessageAsync(InboxMessageResponse message, Exception? exception,IDbTransaction transaction, IDbConnection dbConnection)
    {
        var sql = $"""
                   UPDATE system.inbox_messages
                        SET processed_on_utc =@processed_on_utc,
                            error = @error,
                        WHERE id = @id";"
                   """;
        var result =await dbConnection.ExecuteAsync(sql, new
        {
            processed_on_utc = dateTimeProvider.UtcNow,
            error = exception?.Message,
            id = message.Id
        },transaction);
        
    }
    
    
}