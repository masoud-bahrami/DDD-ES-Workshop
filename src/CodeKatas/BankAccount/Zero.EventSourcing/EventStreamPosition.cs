namespace Zero.EventSourcing
{
    public class EventStreamPosition
    {
        public static EventStreamPosition AtStart() => new EventStreamPosition(0, 0);
        public static EventStreamPosition AtEnd() => new EventStreamPosition(ulong.MaxValue, ulong.MaxValue);
        /// <summary>
        /// The commit positions of the record
        /// </summary>
        public ulong CommitPosition;

        /// <summary>
        /// The prepare positions of the record.
        /// </summary>
        public ulong PreparePosition;

        public EventStreamPosition(ulong commitPosition, ulong preparePosition)
        {
            CommitPosition = commitPosition;
            PreparePosition = preparePosition;
        }

    }
}