using System.Diagnostics;
using System.Text;
using EventStore.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zero.Domain;
using EventData = EventStore.Client.EventData;
using ResolvedEvent = EventStore.Client.ResolvedEvent;
using StreamPosition = EventStore.Client.StreamPosition;

namespace Zero.EventSourcing.EventStoreDb;

public class EventStoreDbEventStore : IEventStore

{

    // EventStore.ClusterNode.exe --db ./db --log ./logs --insecure

    // https://www.eventstore.com/downloads
    // https://github.com/EventStore/EventStore-Client-Dotnet
    // TCP
    // gRPC


    private readonly EventStoreClient _eventStoreDbClient;
    private readonly ILogger<EventStoreDbEventStore> _logger;

    public EventStoreDbEventStore(EventStoreDbConfig config, ILogger<EventStoreDbEventStore> logger)
    {
        _logger = logger;
        var connectionString = config.GetConnectionString();

        var settings = EventStoreClientSettings.Create(connectionString);
        settings.ConnectionName = config.ConnectionName;

        _eventStoreDbClient = new EventStoreClient(settings);

        var conn = _eventStoreDbClient.ConnectionName;
        _logger = logger;
    }

    public async Task<PagedEventStreamViewModel> LoadEventStreamAsync<T>(T streamId,
        EventStreamPosition position,
        EventStreamVersion version = EventStreamVersion.Any
        , int maxCount = 1500) where T : IsAnIdentity
    {
        var eventStreamId = EventStreamId(streamId);

        if (await StreamIsDeleted())
            return PagedEventStreamViewModel.Empty();

        var result = PagedEventStreamViewModel.Empty();

        position ??= EventStreamPosition.AtStart();

        try
        {
            await CheckIfStreamIsMoved();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return PagedEventStreamViewModel.Empty();
        }

        var readStreamAsync = _eventStoreDbClient.ReadStreamAsync(
            Direction.Forwards,
            eventStreamId,
            new StreamPosition(position.CommitPosition), maxCount: maxCount,
            cancellationToken: default);
        try
        {
            var events = await readStreamAsync.ToListAsync(CancellationToken.None);

            foreach (var e in events)
            {
                var eventViewModel = CreateEventViewModel(e);

                // skip StreamWasMigratedFromDomainEvent
                if (eventViewModel.EventType != typeof(StreamWasMigratedFromDomainEvent).AssemblyQualifiedName)
                    result.Events.Add(eventViewModel);
            }
        }
        catch (StreamNotFoundException exception)
        {
            Console.WriteLine(exception);
            return PagedEventStreamViewModel.Empty();
        }
        catch (InvalidOperationException ex)
        {
            LogException(ex);

            //Probably the event position is wrong!
            throw new EventPositionIsWrongException(eventStreamId, position);
        }

        return result;

        async Task<bool> StreamIsDeleted()
        {
            var streamMetadata = await _eventStoreDbClient.GetStreamMetadataAsync(eventStreamId);
            return streamMetadata.StreamDeleted;
        }

        async Task CheckIfStreamIsMoved()
        {
            var latEvent = await _eventStoreDbClient.ReadStreamAsync(Direction.Backwards,
                    eventStreamId,
                    StreamPosition.End
                    , maxCount: 1
                    , cancellationToken: default)
                .FirstOrDefaultAsync();

            if (CreateEventViewModel(latEvent).EventType == typeof(StreamWasMovedToEvent).AssemblyQualifiedName)
                eventStreamId = ((StreamWasMovedToEvent)CreateEventViewModel(latEvent).Payload).To;
        }
    }



    public async Task<PagedEventStreamViewModel> LoadEventsFromAllEventStreamAsync(EventStreamPosition position, int maxCount = 1500)
    {
        var result = PagedEventStreamViewModel.Empty();
        var resolvedEventTask = _eventStoreDbClient.ReadAllAsync(Direction.Forwards,
            position: new EventStore.Client.Position(position.CommitPosition, position.PreparePosition), maxCount: maxCount);

        try
        {
            resolvedEventTask = resolvedEventTask.Where(IsNotSystemEvent);
            var resolvedEvents = await resolvedEventTask.ToListAsync(CancellationToken.None);

            foreach (var eventViewModel in resolvedEvents.Select(CreateEventViewModel))
            {
                result.Events.Add(eventViewModel);
            }
        }
        catch (StreamNotFoundException exception)
        {
            Console.WriteLine(exception);
            return PagedEventStreamViewModel.Empty();
        }

        return result;
    }

    public async Task<EventViewModel> LoadEventAsync<T>(T streamId, EventStreamPosition eventStreamPosition) where T : IsAnIdentity
    {
        var eventStream = await LoadEventStreamAsync(streamId, eventStreamPosition, maxCount: 1);
        if (!eventStream.Events.Any())
            throw new EventStreamNotExistsException(EventStreamId(streamId));

        return eventStream.Events.FirstOrDefault(e => e.PositionAtItsOwnEventStream == eventStreamPosition.CommitPosition);
    }

    public Task<EventViewModel> LoadEventAsync<T>(T streamId, string globalUniqueEventId) where T : IsAnIdentity
    {
        throw new NotImplementedException();
    }

    public Task<PagedEventStreamViewModel> LoadEventsFromAllEventStreamBackwardAsync(EventStreamPositions positions, int maxCount)
    {
        throw new NotImplementedException();
    }


    public Task<EventStreamViewModel> LoadEventStreamAsync<T>(T streamId, DateTimeOffset from, DateTimeOffset to) where T : IsAnIdentity
    {
        throw new NotImplementedException();
    }

    public Task<PagedEventStreamViewModel> LoadEventStreamBackwardAsync<T>(T streamId, EventStreamVersion version = EventStreamVersion.Any, EventStreamPositions positions = EventStreamPositions.FromStart, int maxCount = 1500) where T : IsAnIdentity
    {
        throw new NotImplementedException();
    }


    public async Task AppendToEventStreamAsync<T>(T eventStreamId, AppendEventDto @event, int expectedVersion) where T : IsAnIdentity
    {
        if (@event.EventType.IsADeleteDomainEventEvent())
        {
            await _eventStoreDbClient.SoftDeleteAsync(EventStreamId(eventStreamId), StreamState.Any);
            return;
        }

        var eventData = new EventData(Uuid.NewUuid(), @event.EventType, StringToUTF8BytesConverter(Serializer<T>(@event)));

        var result = expectedVersion == 0 ?
            await _eventStoreDbClient.AppendToStreamAsync(
                EventStreamId(eventStreamId),
                StreamState.Any,
                new[] { eventData },
                cancellationToken: CancellationToken.None
            )

            : await _eventStoreDbClient.AppendToStreamAsync(EventStreamId(eventStreamId), StreamRevision.FromInt64(expectedVersion), new[] { eventData }, cancellationToken: CancellationToken.None);

        if (@event.EventType.IsADeleteDomainEventEvent())
            await _eventStoreDbClient.SoftDeleteAsync(EventStreamId(eventStreamId), StreamState.Any);
    }

    public async Task AppendToEventStreamAsync<T>(T eventStreamId, ICollection<AppendEventDto> events,
        int expectedVersion) where T : IsAnIdentity
    {
        if (events.Any(@event => @event.EventType.IsADeleteDomainEventEvent()))
        {
            await _eventStoreDbClient.SoftDeleteAsync(EventStreamId(eventStreamId), StreamState.Any);
            return;
        }

        var eventDataList = events.Select(appendEventDto =>
                new EventData(Uuid.Parse(appendEventDto.GlobalUniqueEventId),
                    appendEventDto.EventType,
                    StringToUTF8BytesConverter(Serializer<T>(appendEventDto))))
            .ToList();

        if (expectedVersion == 0)
        {
            await _eventStoreDbClient.AppendToStreamAsync(EventStreamId(eventStreamId), StreamState.Any, eventDataList, cancellationToken: CancellationToken.None);
        }
        else
        {
            await _eventStoreDbClient.AppendToStreamAsync(EventStreamId(eventStreamId), StreamRevision.FromInt64(expectedVersion), eventDataList, cancellationToken: CancellationToken.None);
        }

        if (events.Any(@event => @event.EventType.IsADeleteDomainEventEvent()))
            await _eventStoreDbClient.SoftDeleteAsync(EventStreamId(eventStreamId), StreamRevision.None);
    }


    public async Task<EventStreamMetaData> EventStreamMetaDataAsync<T>(T key) where T : IsAnIdentity
    {
        var result = new EventStreamMetaData();
        var metadata = await _eventStoreDbClient.GetStreamMetadataAsync(EventStreamId(key));

        result.MarkAsDeleted = metadata.StreamDeleted;

        return result;
    }

    public async Task<long> GetCurrentVersionOfStream(IsAnIdentity eventStreamId)
    {
        var readStreamResult = _eventStoreDbClient.ReadStreamAsync(Direction.Backwards
            , EventStreamId(eventStreamId),
            StreamPosition.End,
            1, resolveLinkTos: false);

        List<ResolvedEvent> resolvedEvents;
        try
        {
            resolvedEvents = await readStreamResult.ToListAsync();
        }
        catch (StreamNotFoundException exception)
        {
            return 0;
        }

        return resolvedEvents.Any() ? resolvedEvents.First().Event.EventNumber.ToInt64() : 0;
    }

    public async Task SubscribeToAllEventStream(ISubscriber subscriber)
    {
        _logger.LogInformation($"Trying to subscribe to $all event store. - Live Subscription.");

        var subscribeToAllAsync = await _eventStoreDbClient.SubscribeToAllAsync(GetEventAppeared(subscriber, new EventStreamPosition(0, 0)),
            true,
            GetSubscriptionDropped(subscriber),
            new SubscriptionFilterOptions(EventStore.Client.EventTypeFilter.ExcludeSystemEvents()),
            GetEventStoreClientOperationOptions());

        _logger.LogInformation($"Subscription id is {subscribeToAllAsync.SubscriptionId}");
    }

    public async Task CatchUpSubscribeToAllEventStream(ICatchUpSubscriber subscriber,
        EventStreamPosition eventStreamPosition)
    {
        _logger.LogInformation($"Trying to subscribe to $all event store from checkpoint commit position at {eventStreamPosition.CommitPosition} and pre-commit position at {eventStreamPosition.PreparePosition}.");

        var result = await _eventStoreDbClient.SubscribeToAllAsync(
            start: new EventStore.Client.Position(eventStreamPosition.CommitPosition, eventStreamPosition.PreparePosition),
            eventAppeared: GetEventAppeared(subscriber, eventStreamPosition),
            resolveLinkTos: true,
            subscriptionDropped: GetSubscriptionDropped(subscriber, eventStreamPosition),
            filterOptions: new SubscriptionFilterOptions(
                EventStore.Client.EventTypeFilter.ExcludeSystemEvents()),
            configureOperationOptions: GetEventStoreClientOperationOptions());

        _logger.LogInformation($"Subscription id is {result.SubscriptionId}");
    }

    private Action<StreamSubscription, SubscriptionDroppedReason, Exception> GetSubscriptionDropped(
        ISubscriber subscriber)
    {
        return (streamSubscription, reason, exception) =>
        {
            _logger.LogCritical($"{streamSubscription.SubscriptionId} is dropped! Reason : {reason} and exception @{exception}", exception);
        };
    }

    private Action<StreamSubscription, SubscriptionDroppedReason, Exception?> GetSubscriptionDropped(ICatchUpSubscriber subscriber, EventStreamPosition eventStreamPosition)
    {
        return (streamSubscription, reason, exception) =>
        {
            _logger.LogCritical($"{streamSubscription.SubscriptionId} is dropped! Reason : {reason} and exception @{exception}", exception);

            _logger.LogInformation($"Trying to re-subscribe to $all event store from checkpoint commit position at : {eventStreamPosition.CommitPosition} and pre-commit position at {eventStreamPosition.PreparePosition}.");

            CatchUpSubscribeToAllEventStream(subscriber, eventStreamPosition);
        };
    }

    private static Action<EventStoreClientOperationOptions> GetEventStoreClientOperationOptions()
    {
        return e =>
        {
        };
    }

    public Task CatchUpSubscribeToEventStream<T>(T eventStreamId, ICatchUpSubscriber subscriber,
        EventStreamPositions startFrom) where T : IsAnIdentity
    {
        throw new NotImplementedException();
    }




    static object tokenObject = new object();

    public Task SubscribeToEventStream<T>(T eventStreamId, ISubscriber subscriber) where T : IsAnIdentity
    {
        return Task.CompletedTask;
    }


    private Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> GetEventAppeared(ISubscriber subscriber
        , EventStreamPosition eventStreamPosition)
    {

        return (streamSubscription, resolvedEvent, token) =>
        {
            lock (tokenObject)
            {
                eventStreamPosition = new EventStreamPosition(resolvedEvent.Event.Position.CommitPosition,
                    resolvedEvent.Event.Position.PreparePosition);

                _logger.LogInformation(
                    $"An event appeared from stream {resolvedEvent.OriginalStreamId}. EventId is {resolvedEvent.OriginalEvent.EventId}, original position is {resolvedEvent.OriginalPosition.Value}");

                var targetType = Type.GetType(resolvedEvent.OriginalEvent.EventType);
                if (targetType == null)
                {
                    var paramName = $"Can not find {resolvedEvent.OriginalEvent.EventType}";

                    _logger.LogInformation(paramName);

                    _logger.LogError(paramName);

                    Console.WriteLine(paramName);
                    Debug.WriteLine(paramName);
                }

                try
                {
                    subscriber.AnEventAppended(CreateEventViewModel(resolvedEvent));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "An error occurred subscriber.AnEventAppended(ToEventViewMode(resolvedEvent));");
                }

                return Task.CompletedTask;
            }
        };
    }


    private static EventViewModel CreateEventViewModel(ResolvedEvent e)
    {

        return new EventViewModel
        {
            EventType = e.Event.EventType,
            EventId = e.Event.EventId.ToString(),
            Metadata = "",
            GlobalCommitPosition = e.OriginalEvent.Position.CommitPosition,
            GlobalPreparePosition = e.OriginalEvent.Position.PreparePosition,
            PositionAtItsOwnEventStream = e.OriginalEventNumber,
            Payload = ToPayload(e.OriginalEvent),
            Version = 1
        };
    }

    private static byte[] StringToUTF8BytesConverter(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }

    private static object Deserializer(AppendEventDto appendEventDto, Type targetType)
    {
        return JsonConvert.DeserializeObject(appendEventDto.Payload, targetType);
    }

    private static AppendEventDto Deserializer(string appendEventString)
        => JsonConvert.DeserializeObject<AppendEventDto>(appendEventString);
    private static bool IsNotSystemEvent(ResolvedEvent re) => !re.OriginalStreamId.Contains("$");

    private static IsADomainEvent ToPayload(EventRecord eventRecord)
    {
        var appendEventString = Encoding.UTF8.GetString(eventRecord.Data.ToArray());

        var appendEventDto = Deserializer(appendEventString);

        var targetType = Type.GetType(eventRecord.EventType);
        if (targetType == null)
        {
            return null;

            throw new ArgumentNullException($"Can not find {eventRecord.EventType}");
        }

        var deserializedObject = Deserializer(appendEventDto, targetType);

        return deserializedObject as IsADomainEvent;
    }

    private void LogException(Exception ex)
    {
    }

    private static string Serializer<T>(AppendEventDto @event) where T : IsAnIdentity
    {
        return JsonConvert.SerializeObject(@event, new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented
        });
    }


    private string EventStreamId<T>(T streamId) where T : IsAnIdentity
        => streamId.ToString();
}