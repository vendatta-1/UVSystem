using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UVS.Common.Application.Messaging;

namespace UVS.Common.Infrastructure.Outbox;

public static class DomainEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

    public static IEnumerable<IDomainEventHandler> GetHandlers(
        Type type,
        IServiceProvider serviceProvider,
        Assembly assembly)
    {

        Type[] domainEventHandlers = HandlersDictionary.GetOrAdd(
            $"{assembly.GetName()}{type.Name}",
            _ =>
            {
                Type[] handlers = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type)))
                    .ToArray();
                return handlers;
            });

        List<IDomainEventHandler> handlers = [];

        foreach (var handler in domainEventHandlers)
        {
            object domainEventHandler = serviceProvider.GetRequiredService(handler);
            handlers.Add((domainEventHandler as IDomainEventHandler)!);
        }

        return handlers;


    }
}
