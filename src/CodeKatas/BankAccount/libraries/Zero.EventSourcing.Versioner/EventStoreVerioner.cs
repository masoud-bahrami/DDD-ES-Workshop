using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero.Domain;

namespace Zero.EventSourcing.Versioner;

public class EventStoreVerioner : IEventStoreVerioner
{
    private readonly IEventStore _eventStore;
    private readonly IEventTransformerRegistrar _registrar;
    public EventStoreVerioner(IEventStore eventStore, IEventTransformerRegistrar registrar)
    {
        _eventStore = eventStore;
        _registrar = registrar;
    }

    public async Task CopyAndReplaceEventStream(IsAnIdentity from, IsAnIdentity to, bool deleteOldStream = false)
    {
        var pagedEventStreamViewModel = await LoadEventStreamAsync(from);
        var movedFromDomainEvent = new StreamWasMigratedFromDomainEvent(from.ToString(), from.ToString());

        var domainEvents
            = new List<IsADomainEvent>
            {
                movedFromDomainEvent
            };

        domainEvents.AddRange(pagedEventStreamViewModel.Payloads);

        await Copy(to, domainEvents);

        if (deleteOldStream)
            await DeleteEventStream(from);
        else
            await AppendMovedToEventToTheOfStream(from, to);
    }

   
    public async Task CopyTransformAndReplaceEventStream(IsAnIdentity from, IsAnIdentity to, bool deleteOldStream)
    {
        var pagedEventStreamViewModel = await _eventStore.LoadEventStreamAsync(from);
        GuardAgainstEmptyOrNotExistStream(pagedEventStreamViewModel, from);

        await TransformAndCopy(to, pagedEventStreamViewModel);

        if (deleteOldStream)
            await DeleteEventStream(from);
        else
            await AppendMovedToEventToTheOfStream(from, to);
    }

    private async Task<PagedEventStreamViewModel> LoadEventStreamAsync(IsAnIdentity from)
    {
        var pagedEventStreamViewModel = await _eventStore.LoadEventStreamAsync(from);
        GuardAgainstEmptyOrNotExistStream(pagedEventStreamViewModel, from);

        return pagedEventStreamViewModel;
    }
    private async Task DeleteEventStream(IsAnIdentity from)
        => await _eventStore.AppendToEventStreamAsync(from, AppendEventDto.Version1(new StreamWasDeletedEvent(from.ToString())));

    private async Task AppendMovedToEventToTheOfStream(IsAnIdentity from, IsAnIdentity to)
        => await _eventStore.AppendToEventStreamAsync(from, AppendEventDto.Version1(new StreamWasMovedToEvent(from.ToString(), to.ToString())));
    
    private async Task Copy(IsAnIdentity to, ICollection<IsADomainEvent> domainEvents)
        => await _eventStore.AppendToEventStreamAsync(to, domainEvents.Select(AppendEventDto.Version1).ToList());

    private async Task TransformAndCopy(IsAnIdentity to, PagedEventStreamViewModel pagedEventStreamViewModel)
    {
        List<IsADomainEvent> events = new List<IsADomainEvent>();

        foreach (var eventViewModel in pagedEventStreamViewModel.Events)
        {
            var transformer = _registrar.GetTransformerOf(Type.GetType(eventViewModel.EventType));

            var methodInfo = transformer.GetType()
                .GetMethod("Transform");

            var transformedDomainEvents = methodInfo.Invoke(transformer, new[] { eventViewModel.Payload });

            var isADomainEvents = (List<IsADomainEvent>)transformedDomainEvents;

            events.AddRange(isADomainEvents);
        }

        await _eventStore.AppendToEventStreamAsync(to, events.Select(AppendEventDto.Version1).ToList());
    }

    private void GuardAgainstEmptyOrNotExistStream(PagedEventStreamViewModel pagedEventStreamViewModel, IsAnIdentity from)
    {
        if (pagedEventStreamViewModel.Count == 0 || !pagedEventStreamViewModel.Payloads.Any())
            throw new CopyAnEmptyOrNotExistEventStreamException(from.ToString());
    }
}