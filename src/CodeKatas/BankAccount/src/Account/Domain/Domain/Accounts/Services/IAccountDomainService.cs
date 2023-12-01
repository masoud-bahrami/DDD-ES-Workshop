namespace BankAccount.Domain.Accounts.Services;

// bad-smell
public interface IAccountDomainService
{
    void GuardAgainstInitialAmount(decimal initialAmount);
}