using Bank.Account.Domain.Contracts.Events;
using BankAccount.Domain.Accounts.Memento;
using BankAccount.Domain.Accounts.Services;
using Zero.Domain;

namespace BankAccount.Domain.Accounts;
public class Bank : ValueObject<Bank>
{
    public string Name { get; private set; }
    public string Branch { get; private set; }

    public Bank(string name, string branch)
    {
        Name = name;
        Branch = branch;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Branch;
    }
}
//public class Owner : ValueObject<Owner>
//{
//    public string OwnerId { get; set; } = "masoud-124";
//    public string UnituqeUrl { get; set; } = "api/users/masoud-124";

//    protected override IEnumerable<object> GetEqualityComponents()
//    {
//        throw new NotImplementedException();
//    }
//}

public class Account : AggregateRoot<AccountId>
{
    public Transactions Transactions { get; private set; } = Transactions.Init();
    public Bank OpenedIn { get; private set; }

    private Account() : base(null)
    {
    }

    public Account(AccountId accountIdentity,
        decimal initialAmount,
        IAccountDomainService accountDomainService,
        IBankFeesDomainService bankFeesDomainService)
                : base(accountIdentity)
    {
        accountDomainService.GuardAgainstInitialAmount(initialAmount);

        // event-sourced

        Transactions.Add(OpeningAccountTransaction.New(Money.Rial(initialAmount)));
        var transactions = bankFeesDomainService.CalculateFeesOfNewOpeningAccount();
        Transactions.Add(transactions);

        OpenedIn = new Bank("Melli", "2472");

        Apply(ANewAccountHasBeenOpenedDomainEvent.New(Identity.Id));
    }

    //public void Deposit(DepositCommand cmd)
    //{
    //    // TODO check business rules
    //    // TODO handle deposit

    //    // Apply(AAccountHasBeenDeposited);
    //}
    public AccountMemento TakeMemento() => new(base.Identity.Id, Transactions, OpenedIn);

    public static Account Reconstituute(AccountMemento memnto)
    {
        return new Account
        {
            Identity = AccountId.New(memnto.Id),
            Transactions = memnto.Transactions,
            OpenedIn = new Bank(memnto.OpenedIn.Name, memnto.OpenedIn.Branch)
        };
    }

    public Money Balance()
        => Transactions.Balance();

    public static Account Reconstituute(Queue<IsADomainEvent> events)
    {
        throw new NotImplementedException();
    }
}