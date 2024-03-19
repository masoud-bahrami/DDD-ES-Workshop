using Bank.Account.Domain.Contracts.Events;
using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Memento;
using Zero.DataBase;
using Zero.Domain;
using Zero.EventSourcing.Projection;
using Zero.EventSourcing.SqlServerProjector;

namespace BankAccount.Reporting.DataAccess;

public class AccountProjector : ImAProjector
{
    private readonly ZeroDbContext _zeroDbContext;

    public AccountProjector(ZeroDbContext zeroDbContext)
        => _zeroDbContext = zeroDbContext;

    protected override DbOperationCommand Transform(IsADomainEvent domainEvent) 
        => On((dynamic)domainEvent);

    public DbOperationCommand On(ANewAccountHasBeenOpenedDomainEvent @event)
        => new DbAddOperation<AccountMemento>(_zeroDbContext, new AccountMemento())
            .Add(acc =>
            {
                acc.Id = @event.AggregateId;
                acc.OpenedIn = new BankAccount.Domain.Accounts.Bank("Melli", "Fatemi");
                acc.Transactions.Add(OpeningAccountTransaction.New(Money.Rial(@event.InitialAmount)));
            });

    public DbOperationCommand On(SmsFeesTransactionIsAppliedDomainEvent @event)
    => new DbUpdateOperation<AccountMemento>(_zeroDbContext, 
            acc => acc.Id == @event.AggregateId)
            .With(acc =>
            {
                acc.Transactions.Add(SmsFeesTransaction.New(Money.Rial(@event.Amount)));
            });

    public DbOperationCommand On(BankChargesTransactionIsAppliedDomainEvent @event)
        => new DbUpdateOperation<AccountMemento>(_zeroDbContext, acc => acc.Id == @event.AggregateId)
            .With(acc =>
            {
                acc.Transactions.Add(BankChargesTransaction.New(Money.Rial(@event.Amount)));
            });
}