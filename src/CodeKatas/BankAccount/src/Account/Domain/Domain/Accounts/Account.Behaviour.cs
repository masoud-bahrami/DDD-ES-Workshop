using Bank.Account.Domain.Contracts.Events;
using Zero.Domain;

namespace BankAccount.Domain.Accounts;

public partial class Account : AggregateRoot<AccountId>
{
    public Account(Queue<IsADomainEvent> events)
        : base(default)
    {
        foreach (var @event in events)
            Mutate(@event);
    }

    private void Mutate(IsADomainEvent @event)
    {
        When((dynamic)@event);
    }

    private void When(ANewAccountHasBeenOpenedDomainEvent @event)
    {
        Identity = AccountId.New(@event.AggregateId);
        Transactions.Add(OpeningAccountTransaction.New(Money.Rial(@event.InitialAmount)));
    }

    private void When(SmsFeesTransactionIsAppliedDomainEvent @event) 
        => Transactions.Add(SmsFeesTransaction.New(Money.Rial(@event.Amount)));

    private void When(BankChargesTransactionIsAppliedDomainEvent @event) 
        => Transactions.Add(BankChargesTransaction.New(Money.Rial(@event.Amount)));
}