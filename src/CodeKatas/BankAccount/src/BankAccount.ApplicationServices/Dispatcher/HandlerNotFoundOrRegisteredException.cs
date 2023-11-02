namespace BankAccount.ApplicationServices.Dispatcher;

public class HandlerNotFoundOrRegisteredException<T> : Exception
where T : IsACommand
{
    public T Handler { get; }

    public HandlerNotFoundOrRegisteredException()
    {
    }
}