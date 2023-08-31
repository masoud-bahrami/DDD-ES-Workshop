namespace CodeKata.CQRS.Tests;

public class WalletTests
{

    // CQS
    // Side-effect
    // Supple-Design

    [Fact]
    public void testWallet()
    {
        // flyweight
        const int initialBalanceAmount = 500;
        var initialBalance = new Money(initialBalanceAmount);

        const string martinFowler = "Martin Fowler";
        const string ericEvans = "Eric Evans";
        const string kentBeck = "Kent Beck";

        var martinFowlerWallet = new Wallet(martinFowler, initialBalance: initialBalance);
        var kentBeckWallet = new Wallet(kentBeck, initialBalance: initialBalance);
        var ericEvansWallet = new Wallet(ericEvans, initialBalance: initialBalance);

        Assert(martinFowler, martinFowlerWallet);
        Assert(kentBeck, kentBeckWallet);
        Assert(ericEvans, ericEvansWallet);




        // side-effect free
        ericEvansWallet.Deposit(new Money(500));

        Xunit.Assert.Equal(1000, ericEvansWallet.Balance.Amount);


        Xunit.Assert.Equal(initialBalanceAmount, martinFowlerWallet.Balance.Amount);
        Xunit.Assert.Equal(initialBalanceAmount, kentBeckWallet.Balance.Amount);

        void Assert(string expectedOwnerName, Wallet wallet)
        {
            Xunit.Assert.Equal(initialBalanceAmount, wallet.Balance.Amount);
            Xunit.Assert.Equal(expectedOwnerName, wallet.Owner);
        }
    }
}

public class Wallet
{
    public string Owner { get; set; }
    public Money Balance { get; set; }

    public Wallet(string owner, Money initialBalance)
    {
        Balance = initialBalance;
        Owner = owner;
    }
    
    // CQS

    // command
    // design-by contract
    public void Deposit(Money money)
    {
        // side-effect 
        // Aliasing

        Balance = Balance.Add(money);

       // Balance.Amount += money.Amount;
    }
}


// Value object - Whole-Object
public class Money
{
    public int Amount { get;private set; }
    public string  currency{ get;private set; }
    public Money(int amount)
    {
        Amount = amount;
    }

    public Money Add(Money addend) 
        => new Money(Amount + addend.Amount);
}