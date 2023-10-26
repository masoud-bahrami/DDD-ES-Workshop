namespace DDD.SuppleDesign.ClosureOfOperations.Money;

public abstract class Expression
{
    public abstract Expression Plus(Expression augend);
    public abstract Expression Times(Expression  expression);
    
    
    
    public abstract Expression Times(int multiplier);



    public abstract Money Reduce(Bank bank, Currency to);
}




//  5 Dollar * (10CHF + 20 Dollar)