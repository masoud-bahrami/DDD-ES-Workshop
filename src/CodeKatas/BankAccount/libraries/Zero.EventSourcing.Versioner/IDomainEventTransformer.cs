using System.Collections.Generic;
using Zero.Domain;

namespace Zero.EventSourcing.Versioner;

public abstract class IDomainEventTransformer<TFrom> 
{
    public abstract List<IsADomainEvent> Transform(TFrom from);
}