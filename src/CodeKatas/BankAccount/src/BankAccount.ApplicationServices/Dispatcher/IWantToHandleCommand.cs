namespace BankAccount.ApplicationServices.Dispatcher;

public abstract class IWantToHandleCommand<T>
    where T : IsACommand
{
    public abstract Task Handle<T>(T command)
        where T : IsACommand;
}