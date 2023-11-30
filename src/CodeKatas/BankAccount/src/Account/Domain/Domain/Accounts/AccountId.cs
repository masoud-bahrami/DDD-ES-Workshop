using Zero.Domain;

namespace BankAccount.Domain.Accounts;

public class AccountId : Identity<string>
{
    public static AccountId New(string value) => new(value);

    private AccountId(string accountId)
        => Id = accountId;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }

    
}