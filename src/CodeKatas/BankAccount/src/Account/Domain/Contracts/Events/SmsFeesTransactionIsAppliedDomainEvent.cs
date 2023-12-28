using Zero.Domain;

namespace Bank.Account.Domain.Contracts.Events;

public class SmsFeesTransactionIsAppliedDomainEvent : IsADomainEvent
{
    public decimal Amount { get; }

    private SmsFeesTransactionIsAppliedDomainEvent(string identity, decimal amount)
        :base(identity)
    {
        Amount = amount;
    }

    public static SmsFeesTransactionIsAppliedDomainEvent New(string identity, decimal amount)
    {
        return new SmsFeesTransactionIsAppliedDomainEvent(identity , amount);
    }
}