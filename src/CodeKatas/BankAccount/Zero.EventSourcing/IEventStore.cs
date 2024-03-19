using Zero.Domain;

namespace Zero.EventSourcing;

public interface IEventStore
{

    Task<PagedEventStreamViewModel> LoadEventStreamAsync<T>(T streamId
        , EventStreamPosition position = null
        , EventStreamVersion version = EventStreamVersion.Any
        , int maxCount = 1500)
        where T : IsAnIdentity;
        
    Task<PagedEventStreamViewModel> LoadEventStreamBackwardAsync<T>(T streamId
        , EventStreamVersion version = EventStreamVersion.Any
        , EventStreamPositions positions = EventStreamPositions.FromStart
        , int maxCount = 1500)
        where T : IsAnIdentity;

    Task<EventStreamViewModel> LoadEventStreamAsync<T>(T streamId, DateTimeOffset from, DateTimeOffset to)
        where T : IsAnIdentity;

    Task<PagedEventStreamViewModel> LoadEventsFromAllEventStreamAsync(EventStreamPosition position, int maxCount = 1500);

    Task<PagedEventStreamViewModel> LoadEventsFromAllEventStreamBackwardAsync(EventStreamPositions positions, int maxCount);

    Task<EventViewModel> LoadEventAsync<T>(T streamId, EventStreamPosition position)
        where T : IsAnIdentity;

    Task<EventViewModel> LoadEventAsync<T>(T streamId, string globalUniqueEventId)
        where T : IsAnIdentity;

    Task AppendToEventStreamAsync<T>(T eventStreamId, AppendEventDto @event, int expectedVersion = (int)EventStreamVersion.Any)
        where T : IsAnIdentity;
    Task AppendToEventStreamAsync<T>(T eventStreamId, ICollection<AppendEventDto> events, int expectedVersion = (int)EventStreamVersion.Any)
        where T : IsAnIdentity;

    Task SubscribeToEventStream<T>(T eventStreamId, ISubscriber subscriber)
        where T : IsAnIdentity;

    Task SubscribeToAllEventStream(ISubscriber subscriber);
    Task CatchUpSubscribeToAllEventStream(ICatchUpSubscriber subscriber, EventStreamPosition startFrom);
    Task CatchUpSubscribeToEventStream<T>(T eventStreamId, ICatchUpSubscriber subscriber,
        EventStreamPositions startFrom)
        where T : IsAnIdentity;



    Task<EventStreamMetaData> EventStreamMetaDataAsync<T>(T key)
        where T : IsAnIdentity;

    Task<long> GetCurrentVersionOfStream(IsAnIdentity eventStreamId);
}