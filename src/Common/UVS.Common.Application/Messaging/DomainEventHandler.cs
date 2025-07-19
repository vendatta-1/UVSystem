using UVS.Common.Domain;

namespace UVS.Common.Application.Messaging;

public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    public abstract Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
    
    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
      => Handle((TDomainEvent)domainEvent, cancellationToken);
}