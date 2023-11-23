namespace Zero.Domain;

public class AggregateRoot<TId> : Entity<TId> where TId : Identity
{
    public AggregateRoot(TId id) : base(id)
    {
    }
}