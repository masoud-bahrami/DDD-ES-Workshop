using System.Collections.Generic;

namespace Zero.Domain;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : IsAnIdentity
{
    private readonly Queue<IsADomainEvent> _events = new();

    protected AggregateRoot(TId identity) : base(identity)
    {
    }

    protected void Apply(IsADomainEvent @event) 
        => _events.Enqueue(@event);

    public Queue<IsADomainEvent> GetEvents()
    {
        var events = new Queue<IsADomainEvent>(_events);
        
        _events.Clear();
        return events;
    }
}