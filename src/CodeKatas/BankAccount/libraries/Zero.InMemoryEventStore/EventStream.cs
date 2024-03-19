using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zero.Domain;

namespace Zero.EventSourcing.InMemoryEventStore
{
    public class EventStream
    {
        private readonly IList<Event> _events;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Dictionary<string, ISubscriber> _subscribers;
        private readonly Dictionary<string, ICatchUpSubscriber> _catchUpSubscribers;
        public EventStreamMetaData MetaData { get; private set; }
        public EventStream(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _events = new List<Event>();
            _subscribers = new Dictionary<string, ISubscriber>();
            _catchUpSubscribers = new Dictionary<string, ICatchUpSubscriber>();
            MetaData = new EventStreamMetaData();
        }

        public int EventCount
            => _events.Count;

        public void Append(AppendEventDto @event)
            => Append(ToEvent(@event, AssignVersion()));

        public void Append(ICollection<AppendEventDto> events)
        {
            var version = AssignVersion();
            foreach (var appendEventDto in events)
                Append(ToEvent(appendEventDto, version));
        }


        internal ICollection<EventViewModel> Events(EventStreamVersion version = EventStreamVersion.Any
                , long position = 0
        , int maxCount = 1500, bool skipSystemEvents = true)
        {
            var queryable = _events.AsQueryable();
            if (skipSystemEvents)
                queryable = queryable.Where(v => v.EventType != typeof(StreamWasMigratedFromDomainEvent).AssemblyQualifiedName);

            return version == EventStreamVersion.Any
                ? queryable
                    .Skip((int)position)
                    .Take(maxCount).AsEnumerable()
                    .Select(ToEventViewModel)
                    .ToList()
                : queryable.Where(v => v.EventStreamTransactionVersion == (int)version)
                    .Skip((int)position)
                    .Take(maxCount).AsEnumerable()
                    .Select(ToEventViewModel).ToList();
        }


        internal bool MarkedAsDeleted()
            => MetaData.MarkAsDeleted;

        private void Append(Event @event)
        {

            _events.Add(@event);

            foreach (var s in _subscribers.Values.ToList())
                //Task.Factory.StartNew(()=> s.AnEventAppended(ToEventViewModel(@event)));
                try
                {
                    s.AnEventAppended(ToEventViewModel(@event));
                }
                catch (Exception e)
                {
                    //TODO
                    Console.WriteLine(e);
                }

        }



        internal void MarkAsDeleted() => MetaData.MarkAsDeleted = true;

        private Event ToEvent(AppendEventDto @event, int eventStreamTransactionVersion)
        {
            return new Event(_dateTimeProvider.UtcDateTimeNow())
            {
                Version = @event.Version,
                EventStreamTransactionVersion = eventStreamTransactionVersion,
                EventType = @event.EventType,
                GlobalUniqueEventId = @event.GlobalUniqueEventId,
                Metadata = @event.Metadata,
                Payload = @event.Payload,
                Position = AssignLastPosition()
            };
        }

        private int AssignVersion()
        {
            ++MetaData.Version;
            return MetaData.Version;
        }

        private ulong AssignLastPosition()
        {
            var currentPosition = MetaData.Positions;
            ++MetaData.Positions;
            return (ulong)currentPosition; ;
        }

        public int GetVersion()
            => MetaData.Version;

        public ICollection<EventViewModel> Events(DateTimeOffset from, DateTimeOffset to)
            => _events.Where(e => e.OccurredAt >= from && e.OccurredAt <= to).Select(ToEventViewModel).ToList();


        private EventViewModel ToEventViewModel(Event @event)
            => new()
            {
                Version = @event.Version,
                EventType = @event.EventType,
                EventId = @event.GlobalUniqueEventId,
                Metadata = @event.Metadata,
                Payload = DeserializeTo(@event.Payload, @event.EventType),
                GlobalCommitPosition = @event.Position,
                GlobalPreparePosition = @event.Position,
                PositionAtItsOwnEventStream = @event.Position
            };

        private IsADomainEvent DeserializeTo(string eventPayload, string eventEventType)
        {
            var type = Type.GetType(eventEventType);
            var deserializeObject = JsonConvert.DeserializeObject(eventPayload, type);
            return (IsADomainEvent)deserializeObject;
        }

        public EventViewModel GetEventAtPosition(EventStreamPosition eventStreamPosition)
        {
            var @event = _events.FirstOrDefault(e => e.Position == eventStreamPosition.CommitPosition);
            return @event != null ? ToEventViewModel(@event) : default;
        }


        public EventViewModel GetEventById(string globalUniqueEventId)
        {
            var @event = _events.FirstOrDefault(e => e.GlobalUniqueEventId == globalUniqueEventId);
            return @event != null ? ToEventViewModel(@event) : default;
        }

        public DateTimeOffset GetLasEventOccurredDateTimeOffset() => _events.Last().OccurredAt;

        public void Subscribe(ISubscriber subscriber)
            => _subscribers[subscriber.Name] = (subscriber);

        public void CatchUpSubscribe(ICatchUpSubscriber subscriber, ulong lastCheckPoint)
        {
            _catchUpSubscribers[subscriber.Name] = subscriber;
            Task.Factory.StartNew(() => StartCatchingUp(subscriber, lastCheckPoint));
        }

        private Task StartCatchingUp(ICatchUpSubscriber catchUpSubscriber, ulong lastCheckpoint)
        {
            if ((int)lastCheckpoint < EventCount)
            {
                var i = (int)lastCheckpoint;
                while (i < EventCount)
                {
                    catchUpSubscriber.AnEventAppended(ToEventViewModel(_events[i]));
                    i++;
                }

            }
            catchUpSubscriber.LiveProcessingStarted();
            DeleteCatchUp(catchUpSubscriber);

            Subscribe(catchUpSubscriber);

            return Task.CompletedTask;
        }

        private void DeleteCatchUp(ICatchUpSubscriber catchUpSubscriber)
        {
            if (_catchUpSubscribers.TryGetValue(catchUpSubscriber.Name, out _))
                _catchUpSubscribers.Remove(catchUpSubscriber.Name);
        }

        public bool IsMovedTo()
        {
            if (Events().Count == 0)
                return false;

            var lastEvent = Events().Last();

            return lastEvent.EventType == typeof(StreamWasMovedToEvent).AssemblyQualifiedName;
        }

        public string GetMovedStreamId()
        {
            var lastEvent = Events().Last();

            if (lastEvent.EventType == typeof(StreamWasMovedToEvent).AssemblyQualifiedName)
            {
                var movedTo = (lastEvent.Payload as StreamWasMovedToEvent).To;
                return movedTo;
            }

            throw new Exception("can not find moved address");
        }
    }
}