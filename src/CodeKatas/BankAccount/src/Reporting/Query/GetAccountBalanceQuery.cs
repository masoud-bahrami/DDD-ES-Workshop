using Zero.Dispatcher.Query;

namespace BankAccount.Reporting.ApplicationServices;

public class GetAccountBalanceQuery : IAmAQuery
{
    public string Owner { get; }

    public GetAccountBalanceQuery(string owner)
    {
        Owner = owner;
    }
}