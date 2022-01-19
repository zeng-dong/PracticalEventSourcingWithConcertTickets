using Core.Events;

namespace Core.Aggregates;

public abstract class Aggregate : Aggregate<Guid>, IAggregate
{
}

public abstract class Aggregate<T> : IAggregate<T> where T : notnull
{
    public T Id { get; protected set; } = default!;

    public int Version { get; protected set; }

    [NonSerialized] private readonly Queue<IEvent> uncommittedEvents = new();

    public IEvent[] DequeueUncommittedEvents()
    {
        var dequeuedEvents = uncommittedEvents.ToArray();

        uncommittedEvents.Clear();

        return dequeuedEvents;
    }

    public void When(object @event)
    {
    }

    protected void Enqueue(IEvent @event)
    {
        uncommittedEvents.Enqueue(@event);
    }
}