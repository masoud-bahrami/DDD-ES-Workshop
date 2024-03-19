using System;
using System.Collections.Generic;
using Zero.Domain;

namespace Zero.EventSourcing.Versioner;

// func(event) => list of events (transoformed)
public interface IEventTransformer<in TFrom>
{
    ICollection<IsADomainEvent> Transform(TFrom from);
}

public class OrderIsCanceled : IsADomainEvent
{
    public DateTime Date { get; set; }
    public OrderIsCanceled(string aggregateId) : base(aggregateId)
    {
    }
}

public class OrderIsCanceled_V2 : OrderIsCanceled
{
    public string Reason { get; set; }
    public OrderIsCanceled_V2(string aggregateId) : base(aggregateId)
    {
    }
}

public class OrderIsCanceledTransformer
    : IEventTransformer<OrderIsCanceled>
{
    public ICollection<IsADomainEvent> Transform(OrderIsCanceled from)
    {
        return new List<IsADomainEvent>()
        {
            new OrderIsCanceled_V2(from.AggregateId)
            {
                Reason = "Default"
            }
        };
    }
}