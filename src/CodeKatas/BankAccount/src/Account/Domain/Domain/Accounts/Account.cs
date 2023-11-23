using BankAccount.Domain.Accounts.Memento;
using BankAccount.Domain.Accounts.Services;
using Zero.Domain;

namespace BankAccount.Domain.Accounts;

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