using System;
using System.Dynamic;

namespace Zero.Domain;

public abstract class IsADomainEvent
{
    public string AggregateId { get; set; }

    public DomainEventMetaData MetaData { get; set; }
    protected IsADomainEvent(string aggregateId)
    {
        AggregateId = aggregateId;
    }
}

public class DomainEventMetaData
{
    public Version Version { get; set; } 
}