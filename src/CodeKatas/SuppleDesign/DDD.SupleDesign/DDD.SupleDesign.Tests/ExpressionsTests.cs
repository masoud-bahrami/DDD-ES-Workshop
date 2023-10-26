using DDD.SuppleDesign.ClosureOfOperations.Money;

namespace DDD.SupleDesign.Tests;

public class ExpressionsTests
{
   
    [Fact]
    public void testCreateDollar()
    {
        
        Expression fiveDollar = Money.Dollar(5);
        Assert.Equal(Money.Dollar(5), fiveDollar);
    }


    // 1 dollar = 2 CHF
    // 20  * 20 = 400 CHF
    // (5 dollar + 10 chf) * (5 dollar + 5 dollar )

    [Fact]
    public void name()
    {
        // context
        var bank = new Bank();
        bank.AddRate(Currency.Dollar, Currency.Franc, 2);

        Expression fiveDollar = Money.Dollar(5);
        Expression tenCHF = Money.Franc(10);

        Expression sum = new Sum(fiveDollar, tenCHF);

        var result = sum.Times(new Sum(fiveDollar, fiveDollar));


        Assert.Equal(Money.Franc(400) , bank.Reduce(result, Currency.Franc));
    }
}