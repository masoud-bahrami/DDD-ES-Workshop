using Zero.Dispatcher.CommandPipeline;
using Zero.Domain;

namespace Zero.Dispatcher.Command;

public abstract class IWantToHandleCommand<T>
    where T : IsACommand
{
    private readonly Dictionary<IsAnIdentity, Queue<IsADomainEvent>> _queuedEvents = new(0);

    protected void QueueDomainEvents(IsAnIdentity identity, Queue<IsADomainEvent> events)
    {
        _queuedEvents[identity] = events;
    }

    public Dictionary<IsAnIdentity, Queue<IsADomainEvent>> GetQueuedEvents() 
        => _queuedEvents;

    public abstract Task Handle(T command);
}