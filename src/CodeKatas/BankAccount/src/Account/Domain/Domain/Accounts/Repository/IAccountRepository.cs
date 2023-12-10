namespace BankAccount.Domain.Accounts.Repository;

public interface IAccountRepository
{
    Task Store(Account account);
    Task<Account> Reconstitute(AccountId id);
}