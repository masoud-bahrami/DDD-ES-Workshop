namespace Zero.Dispatcher.Command;

public class HandlerNotFoundOrRegisteredException<T> : Exception
where T : IsACommand
{
    public T Handler { get; }

    public HandlerNotFoundOrRegisteredException()
    {
    }
}