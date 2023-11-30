namespace BankAccount.Domain.Accounts.Services;

public interface IAccountIdGeneratorDomainService
{
    AccountId CreateNewAccountId();
}