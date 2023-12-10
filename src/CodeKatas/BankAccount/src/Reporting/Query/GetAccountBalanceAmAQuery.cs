using Zero.Dispatcher.Query;

namespace BankAccount.ApplicationServices.Query;

public class GetAccountBalanceAmAQuery : IAmAQuery
{
    public string Owner { get; }

    public GetAccountBalanceAmAQuery(string owner)
    {
        Owner = owner;
    }
}