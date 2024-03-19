using Zero.Domain;

namespace Zero.EventSourcing;

public class StreamWasMigratedFromDomainEvent : IsADomainEvent
{
    public string From { get; }

    public StreamWasMigratedFromDomainEvent(string aggregateId, string from) : base(aggregateId)
        => From = from;
    
}