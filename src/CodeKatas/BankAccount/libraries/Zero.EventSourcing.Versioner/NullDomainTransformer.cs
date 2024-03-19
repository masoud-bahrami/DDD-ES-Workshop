using System.Collections.Generic;
using Zero.Domain;

namespace Zero.EventSourcing.Versioner;

public class NullDomainTransformer : IDomainEventTransformer<IsADomainEvent>
{
    public static NullDomainTransformer New() => new();
    public override List<IsADomainEvent> Transform(IsADomainEvent from)
    {
        return new List<IsADomainEvent>{from};  
    }
}