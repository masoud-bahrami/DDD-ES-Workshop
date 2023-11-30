using BankAccount.Domain.Accounts.Memento;
using BankAccount.Domain.Accounts.Services;
using Zero.Domain;

namespace BankAccount.Domain.Accounts;

public class Account : AggregateRoot<AccountId>
{
    public Transactions Transactions { get; private set; } = Transactions.Init();

    private Account() : base(null)
    {
    }

    public Account(AccountId accountId,
        decimal initialAmount,
        IAccountDomainService accountDomainService)
                : base(accountId)
    {
        accountDomainService.GuardAgainstInitialAmount(initialAmount);

        var transaction = OpeningAccountTransaction.New(Money.Rial(initialAmount));

        Transactions.Add(transaction);
    }


    public AccountMemento TakeMemento() => new(base.Id.Id, Transactions);

    public Money Balance()
    {
        return Transactions.Balance();
    }
}