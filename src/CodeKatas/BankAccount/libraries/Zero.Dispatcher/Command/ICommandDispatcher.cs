namespace Zero.Dispatcher.Command;


// port
public interface ICommandDispatcher
{
    Task Dispatch<T>(T openBankAccountCommand)
        where T : IsACommand;
}