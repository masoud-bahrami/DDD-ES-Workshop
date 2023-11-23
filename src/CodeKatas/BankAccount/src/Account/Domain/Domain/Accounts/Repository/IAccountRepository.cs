namespace BankAccount.Domain.Accounts.Repository;

public interface IAccountRepository
{
    Task Store(Account account);
}