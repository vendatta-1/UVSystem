using MassTransit;
using UVS.Common.Application.EventBus;

namespace UVS.Common.Infrastructure.EventBus;

/// <summary>
/// for handling integration events and messaging between modules
/// </summary>
/// <param name="bus"></param>
internal  sealed class EventBus(IBus bus):IEventBus
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
         return bus.Publish(integrationEvent, cancellationToken);
    }
}