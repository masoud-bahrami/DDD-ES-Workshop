using System.Collections.Concurrent;
using Zero.Domain;

namespace Zero.Dispatcher.CommandPipeline
{
    public class QueuedEvents
    {
        public long ExpectedVersion { get; set; }
        public Queue<IsADomainEvent> Events { get; set; } = new();
    }

    public class StageContext
    {
        private readonly Dictionary<IsAnIdentity, QueuedEvents> _domainEvents = new();

        public Dictionary<Type, List<Action<IsADomainEvent, long, string>>>
            DomainEventListeners = new();


        private readonly ConcurrentDictionary<string, object> _concurrentDictionary = new();

        public Dictionary<IsAnIdentity, QueuedEvents> GetDomainEvents() => _domainEvents;

        public void QueueDomainEvents(IsAnIdentity eventStreamId, Queue<IsADomainEvent> events)
        {
            if (!_domainEvents.ContainsKey(eventStreamId))
                _domainEvents[eventStreamId] = new QueuedEvents();

            foreach (var isADomainEvent in events)
            {
                _domainEvents[eventStreamId].Events.Enqueue(isADomainEvent);
            }
        }

        public void QueueDomainEvents(Dictionary<IsAnIdentity, Queue<IsADomainEvent>> eventStreamId)
        {
            foreach (var keyValuePair in eventStreamId)
            {
                QueueDomainEvents(keyValuePair.Key, keyValuePair.Value);

            }
        }

        public void Set(string key, object value)
            => _concurrentDictionary[key] = value;

        public object Get(string key)
        {
            if (_concurrentDictionary.TryGetValue(key, out var val))
                return val;

            throw new NotFoundValueInContextException(key);
        }

        public T Get<T>(string key)
        {
            return (T)Get(key);
        }

      

        public void AddEventListeners<T>(Action<IsADomainEvent, long, string> action) where T : IsADomainEvent
        {
            DomainEventListeners ??= new Dictionary<Type, List<Action<IsADomainEvent, long, string>>>();

            if (DomainEventListeners.TryGetValue(typeof(T), out List<Action<IsADomainEvent, long, string>> key))
            {
                key.Add(action);
            }
            DomainEventListeners[typeof(T)] = new List<Action<IsADomainEvent, long, string>> { action };

        }

        public void Clear()
        {
            _domainEvents.Clear();
            DomainEventListeners.Clear();
            _concurrentDictionary.Clear();
        }

       
    }
    public class NotFoundValueInContextException : Exception
    {
        public readonly string Key;

        public NotFoundValueInContextException(string key)
            : base($"{key} not found")
        {
            Key = key;
        }
    }
}
