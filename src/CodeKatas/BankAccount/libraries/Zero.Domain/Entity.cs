namespace Zero.Domain;

public abstract class Entity<TId> where TId : IsAnIdentity{

    public virtual TId Identity { get; protected set; }
    private readonly System.Collections.Generic.Queue<IsADomainEvent> _eventQueue;


    protected Entity(TId identity)
    {
        Identity = identity;
    }

    public List<IsADomainEvent> GetQueuedEvents()
    {
        List<IsADomainEvent> list = this._eventQueue.ToList<IsADomainEvent>();
        this.EmptyQueue();
        return list;
    }
    private void EmptyQueue() => this._eventQueue.Clear();

    public override bool Equals(object obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;
        
        if (Identity.Equals(default) || other.Identity.Equals(default))
            return false;

        return Identity.Equals(other.Identity);
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return Identity.GetHashCode();
    }
    
}