using System;
using System.Collections.Generic;

namespace Zero.EventSourcing.InMemoryEventStore
{
    public class EventStreamViewModelFactory
    {

        public static EventStreamViewModel Create(ICollection<EventViewModel> events, EventStream eventStream,
            DateTimeOffset to)
        {
            bool hasAnyEventYet = eventStream.GetLasEventOccurredDateTimeOffset() > to;

            return new EventStreamViewModel
            (events,
                eventStream.GetVersion(),
                hasAnyEventYet
            );
        }

        public static PagedEventStreamViewModel Create(ICollection<EventViewModel> events, EventStream eventStream,
            EventStreamPositions positions = EventStreamPositions.FromStart, int maxCount = 1050)
        {
            var positionOfTheLastEventAppendedToEventStream = eventStream.EventCount;
            bool hasAnyEventYet = false;

            long nextPosition = 0, nextAllowedMaximumCount = 0;

            var positionOfTheLastEventObserved = (int)positions + maxCount;

            if (positionOfTheLastEventObserved < positionOfTheLastEventAppendedToEventStream)
            {
                nextPosition = positionOfTheLastEventObserved + 1;
                var numberOfEventsNotYetObserved = positionOfTheLastEventAppendedToEventStream - (positionOfTheLastEventObserved);
                nextAllowedMaximumCount = 0;
                if (numberOfEventsNotYetObserved > 0)
                {
                    if (numberOfEventsNotYetObserved > maxCount)
                        nextAllowedMaximumCount = (numberOfEventsNotYetObserved / maxCount) * maxCount;
                    else
                        nextAllowedMaximumCount = numberOfEventsNotYetObserved;
                }

                hasAnyEventYet = true;
            }

            var nextPage = new NextPage(nextPosition, nextAllowedMaximumCount);
            int remainingEventCount = positionOfTheLastEventAppendedToEventStream - positionOfTheLastEventObserved;
            return new PagedEventStreamViewModel(events, eventStream.GetVersion(), hasAnyEventYet, remainingEventCount, nextPage);
        }

        public static PagedEventStreamViewModel CreateBackward(ICollection<EventViewModel> events,
            EventStream eventStream, long position, int maxCount)
        {
            var positionOfTheLastEventAppendedToEventStream = 1;
            bool hasAnyEventYet = false;

            int nextPosition = 0, nextAllowedMaximumCount = 0;

            var positionOfTheLastEventObserved = (int)position;

            if (positionOfTheLastEventObserved > positionOfTheLastEventAppendedToEventStream)
            {
                nextPosition = positionOfTheLastEventObserved;

                var numberOfEventsNotYetObserved = nextPosition;

                nextAllowedMaximumCount = 0;
                if (numberOfEventsNotYetObserved > 0)
                {
                    if (nextPosition -  maxCount > 0)
                        nextAllowedMaximumCount = maxCount;
                    else
                        nextAllowedMaximumCount = nextPosition;
                }

                hasAnyEventYet = true;
            }

            var nextPage = new NextPage(nextPosition, nextAllowedMaximumCount);
            int remainingEventCount = nextPosition;
            return new PagedEventStreamViewModel(events, eventStream.GetVersion(), hasAnyEventYet, remainingEventCount, nextPage);
        }
    }
}