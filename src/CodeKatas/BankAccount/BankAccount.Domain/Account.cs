using BankAccount.Domain.NewFolder;

namespace BankAccount.Domain;

public class Account : AggregateRoot<AccountId>
{
    public decimal Amount { get; set; }

    private Account() : base(null)
    {
    }

    public Account(string accountId,
        decimal initialAmount,
        IAccountDomainService accountDomainService)
                : base(new AccountId(accountId))
    {
        accountDomainService.GuardAgainstInitialAmount(initialAmount);

        Amount = initialAmount;
    }


    public AccountMemento TakeMemento() => new(base.Id.Id, Amount);
}