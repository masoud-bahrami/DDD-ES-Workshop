using Zero.Domain;

namespace Zero.EventSourcing
{
    public class EventViewModel
    {
        public EventViewModel()
        {

        }
        public string EventId { get; set; }
        public string EventType { get; set; }
        public int Version { get; set; }
        public string Metadata { get; set; }
        public IsADomainEvent Payload { get; set; }
        public ulong GlobalCommitPosition { get; set; }
        public ulong GlobalPreparePosition { get; set; }
        public ulong PositionAtItsOwnEventStream { get; set; }
    }
}