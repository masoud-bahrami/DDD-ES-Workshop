namespace BankAccount.Domain.Services;

public class AccountDomainService : IAccountDomainService
{
    private readonly decimal _initialAmountThreshold;


    public AccountDomainService(decimal initialAmountThreshold)
    {
        _initialAmountThreshold = initialAmountThreshold;
    }
    public void GuardAgainstInitialAmount(decimal initialAmount)
    {
        if (initialAmount < _initialAmountThreshold)
            throw new Exception();
    }
}