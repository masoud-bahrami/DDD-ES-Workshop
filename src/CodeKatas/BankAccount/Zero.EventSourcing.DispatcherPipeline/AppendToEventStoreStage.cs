using Zero.Dispatcher.CommandPipeline;
using Zero.Domain;

namespace Zero.EventSourcing.DispatcherPipeline;

public class AppendToEventStoreStage : IAmAPipelineStage
{
    private readonly IEventStore _eventStore;

    public AppendToEventStoreStage(IEventStore eventStore)
        => _eventStore = eventStore;

    public override async Task Process<TContext>(TContext command, StageContext context)
    {
        var domainEvents = context.GetDomainEvents();

        foreach (var (eventStreamId, queuedEvents) in domainEvents)
        {
            await _eventStore.AppendToEventStreamAsync(eventStreamId,
                ToAppendEventDtos(queuedEvents.Events));
        }
    }

    private static List<AppendEventDto> ToAppendEventDtos(IEnumerable<IsADomainEvent> queuedEvents)
    {
        return queuedEvents.Select(AppendEventDto.Version1).ToList();
    }
}