namespace Zero.Dispatcher.Command;

public abstract class IWantToHandleCommand<T>
    where T : IsACommand
{
    public abstract Task Handle(T command);
}