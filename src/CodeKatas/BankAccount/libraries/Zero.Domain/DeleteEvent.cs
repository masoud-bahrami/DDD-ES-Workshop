namespace Zero.Domain;

public abstract class DeleteEvent : IsADomainEvent
{
    protected DeleteEvent(string aggregateId) : base(aggregateId)
    { }
}