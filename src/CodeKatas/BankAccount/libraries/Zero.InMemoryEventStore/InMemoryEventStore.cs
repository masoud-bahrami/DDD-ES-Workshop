using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero.Domain;

namespace Zero.EventSourcing.InMemoryEventStore
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<string, EventStream> _streams;
        private readonly IDateTimeProvider _dateTimeProvider;

        //TODO
        //super duper stupid! yet easy usable solution for all stream
        private readonly EventStream _allStream;

        public InMemoryEventStore(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _allStream = new EventStream(_dateTimeProvider);
            _streams = new ConcurrentDictionary<string, EventStream>();
        }

        public async Task<PagedEventStreamViewModel> LoadEventStreamAsync<T>(T streamId
            , EventStreamPosition position
            , EventStreamVersion version = EventStreamVersion.Any
            , int maxCount = 1500)
           where T : IsAnIdentity
        {
            position ??= EventStreamPosition.AtStart();

            var eventStream = GetStream(streamId);

            if (eventStream == null || eventStream.MarkedAsDeleted())
                return PagedEventStreamViewModel.Empty();

            if (eventStream.IsMovedTo())
            {
                string movedTo = eventStream.GetMovedStreamId();
                eventStream = GetStream(movedTo);
            }
            
            return eventStream == null || eventStream.MarkedAsDeleted()
                ? PagedEventStreamViewModel.Empty()
                : EventStreamViewModelFactory.Create(
                    eventStream.Events(version, (long)position.CommitPosition, maxCount), eventStream);
        }

        public async Task<PagedEventStreamViewModel> LoadEventStreamBackwardAsync<T>(T streamId, EventStreamPosition position, EventStreamVersion version = EventStreamVersion.Any,
            int maxCount = 1500) where T : IsAnIdentity
        {
            var eventStream = GetStream(streamId);
            if (eventStream == null)
                return PagedEventStreamViewModel.Empty();

            var eventStreamPosition = (position == EventStreamPosition.AtEnd())
                ? (eventStream.EventCount - maxCount)
                : (long)position.CommitPosition - maxCount;

            var eventViewModels = eventStream.Events(EventStreamVersion.Any, eventStreamPosition, maxCount);

            return EventStreamViewModelFactory.CreateBackward(eventViewModels, eventStream, eventStreamPosition, maxCount);
        }

        public async Task<EventStreamViewModel> LoadEventStreamAsync<T>(T streamId, DateTimeOffset @from, DateTimeOffset to) where T : IsAnIdentity
        {
            var eventStream = GetStream(streamId);
            return eventStream == null
                ? PagedEventStreamViewModel.Empty()
                : EventStreamViewModelFactory.Create(eventStream.Events(@from, to), eventStream, to);
        }

        public async Task<EventViewModel> LoadEventAsync<T>(T streamId, EventStreamPosition eventStreamPosition) where T : IsAnIdentity
        {
            var eventStream = GetStream(streamId);
            if (eventStream == null)
                throw new EventStreamNotExistsException(ToStreamIdString(streamId));

            return eventStream.GetEventAtPosition(eventStreamPosition) ?? throw new EventPositionIsWrongException(ToStreamIdString(streamId), eventStreamPosition);
        }

        public async Task<EventViewModel> LoadEventAsync<T>(T streamId, string globalUniqueEventId) where T : IsAnIdentity
        {
            var eventStream = GetStream(streamId);
            if (eventStream == null)
                throw new EventStreamNotExistsException(ToStreamIdString(streamId));

            return eventStream.GetEventById(globalUniqueEventId) ?? throw new EventPositionIsWrongException(ToStreamIdString(streamId), new EventStreamPosition(1, 1));
        }

        public async Task<PagedEventStreamViewModel> LoadEventsFromAllEventStreamAsync(EventStreamPosition position, int maxCount)
        {
            var eventStream = EventStreamViewModelFactory.Create(_allStream.Events(EventStreamVersion.Any, (long)position.CommitPosition, maxCount), _allStream, EventStreamPositions.FromStart, maxCount);

            return eventStream;
        }

        public async Task<PagedEventStreamViewModel> LoadEventsFromAllEventStreamBackwardAsync(EventStreamPositions positions, int maxCount)
        {
            var eventStreamPosition = (positions == EventStreamPositions.FromEnd)
                ? (EventStreamPositions)(_allStream.EventCount - maxCount)
                : positions - maxCount;

            var eventViewModels = _allStream.Events(EventStreamVersion.Any, (long)eventStreamPosition, maxCount);

            var eventStream = EventStreamViewModelFactory.CreateBackward(eventViewModels, _allStream, 0, maxCount);

            return eventStream;
        }


        public async Task AppendToEventStreamAsync<T>(T eventStreamId, AppendEventDto @event, int expectedVersion = 0)
            where T : IsAnIdentity
        {
            var eventStream = GetOrAddStream(eventStreamId);
            if (eventStream.MarkedAsDeleted())
                throw new AppendingToDeletedEventStreamException(ToStreamIdString(eventStreamId));

            eventStream.Append(@event);
            AppendToAllEventStream(@event);

            if (@event.EventType.IsADeleteDomainEventEvent())
                eventStream.MarkAsDeleted();
        }

        public async Task AppendToEventStreamAsync<T>(T eventStreamId, ICollection<AppendEventDto> events,
            int expectedVersion=0) where T : IsAnIdentity
        {
            var eventStream = GetOrAddStream(eventStreamId);
            eventStream.Append(events);

            AppendToAllEventStream(events);

            if (events.Any(e => e.EventType.IsADeleteDomainEventEvent()))
            {
                eventStream.MarkAsDeleted();
            }
        }


        public Task SubscribeToEventStream<T>(T streamId, ISubscriber subscriber)
            where T : IsAnIdentity
        {
            var eventStream = GetOrAddStream(streamId);
            eventStream.Subscribe(subscriber);
            return Task.CompletedTask;
        }

        public Task SubscribeToAllEventStream(ISubscriber subscriber)
        {
            _allStream.Subscribe(subscriber);
            return Task.CompletedTask;
        }

        public Task CatchUpSubscribeToAllEventStream(ICatchUpSubscriber subscriber, EventStreamPosition lastCheckPoint)
        {
            _allStream.CatchUpSubscribe(subscriber, lastCheckPoint.CommitPosition);
            return Task.CompletedTask;
        }

        public Task CatchUpSubscribeToEventStream<T>(T eventStreamId, ICatchUpSubscriber subscriber,
            EventStreamPositions lastCheckPoint) where T : IsAnIdentity
        {
            var eventStream = GetOrAddStream(eventStreamId);

            eventStream.CatchUpSubscribe(subscriber, (ulong)lastCheckPoint);
            return Task.CompletedTask;
        }

        private void AppendToAllEventStream(AppendEventDto @event)
            => _allStream.Append(@event);

        private void AppendToAllEventStream(ICollection<AppendEventDto> events)
            => _allStream.Append(events);

        private EventStream GetOrAddStream<T>(T streamId) where T : IsAnIdentity
        {
            if (_streams.TryGetValue(streamId.ToString(), out var eventStream))
                return eventStream;

            eventStream = new EventStream(_dateTimeProvider);
            _streams[streamId.ToString()] = eventStream;

            return eventStream;
        }

        private EventStream GetStream<T>(T streamId) where T : IsAnIdentity
        {
            _streams.TryGetValue(ToStreamIdString(streamId), out var eventStream);
            return eventStream;
        }
        private EventStream GetStream(string streamId)
        {
            _streams.TryGetValue(streamId, out var eventStream);
            return eventStream;
        }

        private string ToStreamIdString<T>(T streamId) where T : IsAnIdentity
            => streamId.ToString();

        public async Task<EventStreamMetaData> EventStreamMetaDataAsync<T>(T streamId) where T : IsAnIdentity
        {
            var eventStream = GetStream(streamId);
            if (eventStream == null)
                throw new EventStreamNotExistsException(ToStreamIdString(streamId));
            return eventStream.MetaData;
        }

        public async Task<long> GetCurrentVersionOfStream(IsAnIdentity eventStreamId)
        {
            var eventStream = GetStream(eventStreamId);
            if (eventStream is { EventCount: > 0 })
                return eventStream.Events().Count;

            return 0;
        }

        public Task<PagedEventStreamViewModel> LoadEventStreamBackwardAsync<T>(T streamId, EventStreamVersion version = EventStreamVersion.Any, EventStreamPositions positions = EventStreamPositions.FromStart, int maxCount = 1500) where T : IsAnIdentity
        {
            throw new NotImplementedException();
        }
        
    }
}