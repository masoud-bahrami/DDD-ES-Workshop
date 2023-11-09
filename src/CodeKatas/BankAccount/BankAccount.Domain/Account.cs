namespace BankAccount.Domain;

public class Account
{
    public string AccountId { get; }
    public decimal Amount { get; set; }
    
    public Account(string accountId, decimal initialAmount, IAccountDomainService accountDomainService)
    {
        accountDomainService.GuardAgainstInitialAmount(initialAmount);
        
        AccountId = accountId;
        Amount = initialAmount;
    }

    public override bool Equals(object? obj)
    {
        var that = (Account)obj;
        return that.AccountId == that.AccountId;
    }
}

