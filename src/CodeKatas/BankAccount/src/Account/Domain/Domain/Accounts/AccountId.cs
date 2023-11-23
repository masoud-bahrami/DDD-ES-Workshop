using Zero.Domain;

namespace BankAccount.Domain.Accounts;

public class AccountId : Identity<string>
{
    public AccountId(string accountId)
        => Id = accountId;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}