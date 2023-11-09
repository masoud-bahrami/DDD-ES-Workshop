namespace BankAccount.Domain;

public interface IAccountDomainService
{
    void GuardAgainstInitialAmount(decimal initialAmount);
}