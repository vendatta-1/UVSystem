using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using UVS.Common.Application.Clock;
using UVS.Common.Domain;
using UVS.Common.Infrastructure.Serialization;

namespace UVS.Common.Infrastructure.Outbox;

public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;
    public InsertOutboxMessagesInterceptor(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTimeProvider)
    {
        
        _httpContextAccessor = httpContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
            if (_httpContextAccessor.HttpContext is not null)
                InsertAuditingData(eventData.Context, _httpContextAccessor.HttpContext, _dateTimeProvider);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void InsertAuditingData(DbContext context, HttpContext httpContext, IDateTimeProvider dateTimeProvider)
    {
        var auditEntries = context.ChangeTracker.Entries<AuditEntity>();

        var userId = httpContext?.User?.FindFirst("sub")?.Value
                     ?? httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? "Anonymous";

        foreach (var entry in auditEntries)
        {
            var entity = entry.Entity;
            var now = dateTimeProvider.UtcNow;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = now;
                    entity.CreatedBy = userId;
                    break;

                case EntityState.Modified:
                    entity.UpdatedAt = now;
                    entity.UpdatedBy = userId;
                    break;

                case EntityState.Deleted:
                    entity.DeletedAt = now;
                    entity.DeletedBy = userId;
                    break;
            }
        }
    }


    private static void InsertOutboxMessages(DbContext context)
    {
        var outboxMessages = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = domainEvent.Id,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings.Instance),
                OccurredOnUtc = domainEvent.OccurredOnUtc
            })
            .ToList();
        
        context.Set<OutboxMessage>().AddRange(outboxMessages);
        
    }
}
