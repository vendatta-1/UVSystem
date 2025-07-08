using MassTransit;
using UVS.Common.Application.EventBus;

namespace UVS.Common.Infrastructure.EventBus;

internal  sealed class EventBus(IBus bus):IEventBus
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
         return bus.Publish(integrationEvent, cancellationToken);
    }
}