using System;

namespace Zero.EventSourcing.InMemoryEventStore
{
    [Serializable]
    public class SubscriberNameDuplicationException : Exception
    {
        public SubscriberNameDuplicationException(ISubscriber subscriber)
        {
            Subscriber = subscriber;
        }


        public ISubscriber Subscriber { get; }
    }
}