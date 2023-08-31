namespace CodeKata.TDD.Domain;

public class Money : Expression
{
    public int Amount { get; init; }
    private Currency Currency { get; init; }

    private Money(int amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Franc(int amount)
    {
        return new Money(amount, Currency.Franc);
    }

    public static Money Dollar(int amount)
    {
        return new Money(amount, Currency.Dollar);
    }

    public override bool Equals(object? obj)
    {
        var that = (Money)obj;

        return
            this.GetCurrency() == that.GetCurrency() &&
            this.Amount == that.Amount;
    }

    public override Expression Times(int multiplier)
    {
        return new Money(Amount * multiplier, GetCurrency());
    }

    public Currency GetCurrency()
    {
        return Currency;
    }

    public override  Expression Plus(Expression money)
        => new Sum(this, money);

    public override Money Reduce(Bank bank, Currency to)
    {
        var rate = bank.GetRate(GetCurrency(), to);
        return new Money(Amount / rate, to);
    }
}