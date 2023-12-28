using Zero.Domain;

namespace Bank.Account.Domain.Contracts.Events;

public class BankChargesTransactionIsAppliedDomainEvent : IsADomainEvent
{
    public decimal Amount { get; }

    public BankChargesTransactionIsAppliedDomainEvent(string identity, decimal amount):base(identity)
    {
        Amount = amount;
    }

    public static BankChargesTransactionIsAppliedDomainEvent New(string aggregateId, decimal amount) 
        => new BankChargesTransactionIsAppliedDomainEvent(aggregateId, amount);
}