using UVS.Common.Domain;

namespace UVS.Common.Application.Messaging;

public abstract class DomainEventHandler<TDomainEven> :IDomainEventHandler where TDomainEven : IDomainEvent
{
    public abstract Task Handle(TDomainEven domainEvent, CancellationToken cancellationToken);
    
    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
      => Handle((TDomainEven)domainEvent, cancellationToken);
}