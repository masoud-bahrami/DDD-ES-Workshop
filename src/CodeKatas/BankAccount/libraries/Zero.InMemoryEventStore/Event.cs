using System;

namespace Zero.EventSourcing.InMemoryEventStore
{
    public class Event
    {
        public string GlobalUniqueEventId { get; set; }
        public string EventType { get; set; }
        public int Version { get; set; }
        public int EventStreamTransactionVersion { get; set; }
        public string Metadata { get; set; }
        public string Payload { get; set; }
        public ulong Position { get; set; }
        public DateTimeOffset OccurredAt { get; set; }

        public Event(DateTimeOffset occurredAt)
            => OccurredAt = occurredAt;
    }
}