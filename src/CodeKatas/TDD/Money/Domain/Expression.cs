namespace CodeKata.TDD.Domain;

public abstract class Expression
{
    public abstract Money Reduce(Bank bank, Currency to);
    public abstract Expression Plus(Expression augend);
    public abstract Expression Times(int multiplier);
}