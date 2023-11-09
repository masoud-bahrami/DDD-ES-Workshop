namespace BankAccount.Domain;

public class Money : ValueObject<Money>
{
    public decimal Amount { get; init; }
    public string Curency { get; init; }

    public static Money Rial(decimal amount)
            => new()
            {
                Amount = amount,
                Curency = "Rial"
            };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Curency;
    }
}