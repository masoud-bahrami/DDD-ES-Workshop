namespace DDD.SuppleDesign.ClosureOfOperations.Money;

public class Sum : Expression
{
    private Expression Augend { get; }
    private Expression Addend { get; }

    public Sum(Expression augend, Expression addend)
    {
        Augend = augend;
        Addend = addend;
    }


    public override Money Reduce(Bank bank, Currency to)
    {
        return
        Money.Dollar(Addend.Reduce(bank, to).Amount +
                     Augend.Reduce(bank, to).Amount);
    }

    public override Expression Plus(Expression augend)
    {
        return new Sum(this, augend);
    }

    public override Expression Times(Expression expression)
    {
        throw new NotImplementedException();
    }

    public override Expression Times(int multiplier)
    {
        return new Sum(Augend.Times(multiplier), Addend.Times(multiplier));
    }
}