using BankAccount.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Zero.Dispatcher.Query;

namespace BankAccount.ApplicationServices.Query;

public class GetAccountBalanceQueryHandler : IWantToHandleQuery<GetAccountBalanceQuery , decimal>
{
    private readonly BankAccountDbContext _bankAccountDbContext;

    public GetAccountBalanceQueryHandler(BankAccountDbContext bankAccountDbContext)
    {
        _bankAccountDbContext = bankAccountDbContext;
    }

    public override async Task<decimal> Handle<T>(T query)
    {
        var account = await _bankAccountDbContext.Accounts
            .FirstOrDefaultAsync();

        if (account is null)
            throw new AccountNotFoundException();

        return account.Balance();
    }
}

public class AccountNotFoundException : Exception
{
}