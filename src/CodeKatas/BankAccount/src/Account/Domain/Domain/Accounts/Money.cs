using Zero.Domain;

namespace BankAccount.Domain.Accounts;

public class Money : ValueObject<Money>
{
    public decimal Amount { get; init; }
    public string Currency { get; }

    public static Money Rial(decimal amount)
        => new(amount, "Rial");
    
    private Money(decimal amount, string currency)
    {
        GuardAgainstAmountValue(amount);
        Amount = amount;
        Currency = currency;
    }

    private void GuardAgainstAmountValue(decimal amount)
    {
        if (amount < 0)
            throw new Money.AmountIsLowerThanZeroException(amount);
    }

   

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public class AmountIsLowerThanZeroException : Exception
    {
        public decimal Amount { get; }

        public AmountIsLowerThanZeroException(decimal amount)
        {
            Amount = amount;
        }
    }

}