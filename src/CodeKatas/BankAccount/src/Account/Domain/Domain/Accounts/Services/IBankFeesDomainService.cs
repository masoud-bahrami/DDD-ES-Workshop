namespace BankAccount.Domain.Accounts.Services;

public interface IBankFeesDomainService
{
    IEnumerable<Transaction> CalculateFeesOfNewOpeningAccount();
}