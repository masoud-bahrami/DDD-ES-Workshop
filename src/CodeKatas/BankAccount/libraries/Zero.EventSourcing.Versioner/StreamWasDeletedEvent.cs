using Zero.Domain;

namespace Zero.EventSourcing.Versioner;

public class StreamWasDeletedEvent : DeleteEvent
{
    public StreamWasDeletedEvent(string id) : base(id)
    {
    }
}