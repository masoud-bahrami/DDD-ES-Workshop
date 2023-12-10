using Zero.Domain;

namespace Bank.Account.Domain.Contracts.Events;

public class ANewAccountHasBeenOpenedDomainEvent : IsADomainEvent
{
    public string Id { get; }

    public static ANewAccountHasBeenOpenedDomainEvent New(string id) => new(id);

    private ANewAccountHasBeenOpenedDomainEvent(string id)
    {
        Id = id;
    }

}