using System.Text.Json.Serialization;
using Zero.Domain;

namespace Zero.EventSourcing
{
    public class StreamWasMovedToEvent : IsADomainEvent
    {

        public string To { get; set; }
        public string From { get; set; }

        [JsonConstructor]
        public StreamWasMovedToEvent(string aggregateId, string from, string to) : base(aggregateId)
        {
            From = from;
            To = to;
        }

        public StreamWasMovedToEvent(string from, string to) : this(from, from, to)
        {
        }
        
    }
}