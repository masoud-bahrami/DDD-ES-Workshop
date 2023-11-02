namespace BankAccount.ApplicationServices.Dispatcher;


// port
public interface ICommandDispatcher
{
    Task Dispatch<T>(T openBankAccountCommand)
        where T : IsACommand;
}