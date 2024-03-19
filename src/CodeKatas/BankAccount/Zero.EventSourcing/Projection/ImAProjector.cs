using Zero.Domain;

namespace Zero.EventSourcing.Projection;

public abstract class ImAProjector
{
    
    public async Task Process(IsADomainEvent @event)
    {
        var command = Transform(@event);
        await command.Execute();
    }

    protected abstract DbOperationCommand Transform(IsADomainEvent domainEvent);
}