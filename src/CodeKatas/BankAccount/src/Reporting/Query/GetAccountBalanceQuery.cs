using Zero.Dispatcher.Query;

namespace BankAccount.ApplicationServices.Query;

public class GetAccountBalanceQuery : IQuery
{
    public string Owner { get; }

    public GetAccountBalanceQuery(string owner)
    {
        Owner = owner;
    }
}