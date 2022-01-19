using Core.Aggregates;
using Core.Events;
using Marten;

namespace Core.Marten.Aggregates;

public static class AggregateExtensions
{
    public static async Task StoreAndPublishEvents(
        this IAggregate aggregate,
        IDocumentSession session,
        IEventBus eventBus,
        CancellationToken cancellationToken = default
        )
    {
        var uncommittedEvents = aggregate.DequeueUncommittedEvents();
        session.Events.Append(aggregate.Id, uncommittedEvents);
        await session.SaveChangesAsync(cancellationToken);
        await eventBus.Publish(uncommittedEvents);
    }
}