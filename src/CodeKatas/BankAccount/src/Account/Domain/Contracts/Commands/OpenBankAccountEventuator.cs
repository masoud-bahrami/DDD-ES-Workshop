using Bank.Account.Domain.Contracts.Events;
using Zero.Domain;

namespace Bank.Account.Domain.Contracts.Commands;

public static class OpenBankAccountEventuator
{
    public static Queue<IsADomainEvent> EventuateTo(this OpenBankAccountCommand command, string aggregateId)
    {
        var result = new Queue<IsADomainEvent>();

        ANewAccountHasBeenOpenedDomainEvent.New(aggregateId, command.InitialAmount, "rial");
        SmsFeesTransactionIsAppliedDomainEvent.New(aggregateId, 0M);
        BankChargesTransactionIsAppliedDomainEvent.New(aggregateId, 0M);

        return result;
    }
}