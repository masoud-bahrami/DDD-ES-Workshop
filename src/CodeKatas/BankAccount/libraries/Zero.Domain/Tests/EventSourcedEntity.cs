namespace Zero.Domain.Tests;

public abstract class EventSourcedEntity<TId> : Entity<TId> where TId : IsAnIdentity
{

    private readonly Queue<IsADomainEvent> _queuedEvents;


    protected EventSourcedEntity(Queue<IsADomainEvent> events) : base(default)
    {
        foreach (var isADomainEvent in events)
            Mutate(isADomainEvent);
    }

    protected EventSourcedEntity(TId identity) : base(identity)
    {
    }

    public List<IsADomainEvent> GetQueuedEvents()
    {
        var list = _queuedEvents.ToList();
        EmptyQueue();
        return list;
    }

    protected void Enqueue(IsADomainEvent @event)
        => _queuedEvents.Enqueue(@event);

    protected void Apply(IsADomainEvent @event)
    {
        Mutate(@event);
        Enqueue(@event);
    }

    protected abstract void Mutate(IsADomainEvent @event);
    private void EmptyQueue() => _queuedEvents.Clear();
}