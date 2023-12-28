using Zero.Domain;

namespace Bank.Account.Domain.Contracts.Events;

public class ANewAccountHasBeenOpenedDomainEvent : IsADomainEvent
{
    public decimal InitialAmount { get; }
    public string Currency { get; }
    public string Id { get; }

    public static ANewAccountHasBeenOpenedDomainEvent New(string id, decimal initialAmount, string currency) 
        => new(id, initialAmount , currency);
    
    private ANewAccountHasBeenOpenedDomainEvent(string id, decimal initialAmount, string currency) : base(id)
    {
        InitialAmount = initialAmount;
        Currency = currency;
    }
}