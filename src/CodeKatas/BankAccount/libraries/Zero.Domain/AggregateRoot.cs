namespace Zero.Domain;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : IsAnIdentity
{
    private readonly Queue<IsADomainEvent> _events = new();

    protected AggregateRoot(TId identity) : base(identity)
    {
    }

    protected void Apply(IsADomainEvent @event)
        => _events.Enqueue(@event);

    protected void Apply(Queue<IsADomainEvent> @events)
    {
        foreach (var isADomainEvent in _events) Apply(isADomainEvent);
    }

    public Queue<IsADomainEvent> GetEvents()
    {
        var events = new Queue<IsADomainEvent>(_events);

        _events.Clear();
        return events;
    }
}