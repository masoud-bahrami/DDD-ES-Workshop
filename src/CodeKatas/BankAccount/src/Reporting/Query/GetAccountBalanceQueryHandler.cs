using BankAccount.Domain.Accounts.Memento;
using Microsoft.EntityFrameworkCore;
using Zero.DataBase;
using Zero.Dispatcher.Query;

namespace BankAccount.ApplicationServices.Query;

public class GetAccountBalanceQueryHandler : IWantToHandleQuery<GetAccountBalanceAmAQuery , decimal>
{
    private readonly ZeroDbContext _bankAccountDbContext;

    public GetAccountBalanceQueryHandler(ZeroDbContext bankAccountDbContext) 
        => _bankAccountDbContext = bankAccountDbContext;

    public override async Task<decimal> Handle<T>(T query)
    {
        var account = await _bankAccountDbContext.Set<AccountMemento>()
            .FirstOrDefaultAsync();


        // recomendationService.Get(productId);
        
        return account.Balance();
    }

    public override async Task<decimal> FallBack(GetAccountBalanceAmAQuery query, Exception exception)
    {
        // TODO
        //if (exception is RecServiceUnaExce)
        // default

        //if (account is null)
        //    throw new AccountNotFoundException();

        throw exception;
    }
}