using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Repository;

namespace BankAccount.Infrastructure;

public class AccountRepository : IAccountRepository
{
    private readonly BankAccountDbContext _context;

    public AccountRepository(BankAccountDbContext context)
    {
        _context = context;
    }

    public async Task Store(Account account)
    {
        await _context.Accounts
            .AddAsync(account.TakeMemento());

        // TODO 
        await _context.SaveChangesAsync();
    }
}