namespace Zero.EventSourcing;

public class AppendingToDeletedEventStreamException : Exception
{
    public string EventStreamId;

    public AppendingToDeletedEventStreamException(string eventStreamId)
    {
        EventStreamId = eventStreamId;
    }
}