using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Services;

namespace BankAccount.Domain.Services;

public class AccountIdGeneratorDomainService : IAccountIdGeneratorDomainService
{
    
    public AccountId CreateNewAccountId() 
        => AccountId.New(Guid.NewGuid().ToString());
}