using Bank.Account.Domain.Contracts.Commands;
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

public partial class Account : AggregateRoot<AccountId>
{
    public Transactions Transactions { get; private set; } = Transactions.Init();
    public Bank OpenedIn { get; private set; }

    private Account() : base(null)
    {
    }

    public Account(AccountId accountIdentity,
        OpenBankAccountCommand cmd,
        IAccountDomainService accountDomainService,
        IBankFeesDomainService bankFeesDomainService)
        : base(accountIdentity)
    {
        // transaction
        // event
        accountDomainService.GuardAgainstInitialAmount(cmd.InitialAmount);

        // event-sourced

        // t1 , t2 , t3 , tn , tn+1

        Transactions.Add(OpeningAccountTransaction.New(Money.Rial(cmd.InitialAmount)));
        var transactions = bankFeesDomainService.CalculateFeesOfNewOpeningAccount();
        Transactions.Add(transactions);

        OpenedIn = new Bank("Melli", "2472");

        // break => event
        Apply(ANewAccountHasBeenOpenedDomainEvent.New(Identity.Id, cmd.InitialAmount, "rial"));
        Apply(SmsFeesTransactionIsAppliedDomainEvent.New(Identity.Id , 0M));
        Apply(BankChargesTransactionIsAppliedDomainEvent.New(Identity.Id,0M));

        var isADomainEvents = cmd.EventuateTo(Identity.Id);
        //Apply(isADomainEvents);
    }
    
    public static Account Reconstitute(AccountMemento memento)
    {
        return new Account
        {
            Identity = AccountId.New(memento.Id),
            Transactions = memento.Transactions,
            OpenedIn = new Bank(memento.OpenedIn.Name, memento.OpenedIn.Branch)
        };
    }

    public Money Balance()
        => Transactions.Balance();

    public AccountMemento TakeMemento() => new(base.Identity.Id, Transactions, OpenedIn);

    public static Account Reconstitute(Queue<IsADomainEvent> events)
    {
        throw new NotImplementedException();
    }
}