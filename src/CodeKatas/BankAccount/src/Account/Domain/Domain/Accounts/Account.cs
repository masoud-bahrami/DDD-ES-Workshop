using BankAccount.Domain.Accounts.Memento;
using BankAccount.Domain.Accounts.Services;
using Zero.Domain;

namespace BankAccount.Domain.Accounts;

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
    //public Owner Owner { get; set; }
    public Transactions Transactions { get; private set; } = Transactions.Init();

    private Account() : base(null)
    {
    }

    public Account(AccountId accountId,
        decimal initialAmount, 
        IAccountDomainService accountDomainService,
        IBankFeesDomainService bankFeesDomainService)
                : base(accountId)
    {
        accountDomainService.GuardAgainstInitialAmount(initialAmount);

        // event-sourced

        var transactions = bankFeesDomainService.CalculateFeesOfNewOpeningAccount();

        Transactions.Add(OpeningAccountTransaction.New(Money.Rial(initialAmount)));
        Transactions.Add(transactions);

    }


    public AccountMemento TakeMemento() => new(base.Id.Id, Transactions);

    
    public Money Balance() 
        => Transactions.Balance();
}