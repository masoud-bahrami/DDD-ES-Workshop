namespace BankAccount.Domain.Accounts.Services;

public interface IAccountDomainService
{
    void GuardAgainstInitialAmount(decimal initialAmount);
}