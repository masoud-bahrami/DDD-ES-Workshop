namespace BankAccount.Domain;

public interface IAccountRepository
{
    Task Store(Account account);
}