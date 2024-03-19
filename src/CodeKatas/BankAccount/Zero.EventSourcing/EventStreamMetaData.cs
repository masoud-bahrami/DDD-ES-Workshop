namespace Zero.EventSourcing
{
    public class EventStreamMetaData
    {
        public int Version { get; set; }

        public EventStreamPositions Positions { get; set; }

        public bool MarkAsDeleted { get; set; }
    }
}