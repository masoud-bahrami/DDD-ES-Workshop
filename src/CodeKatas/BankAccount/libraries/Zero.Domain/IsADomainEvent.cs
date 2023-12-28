namespace Zero.Domain;

public abstract class IsADomainEvent
{
    public string AggregateId { get; set; }

    protected IsADomainEvent(string aggregateId)
    {
        AggregateId = aggregateId;
    }
}