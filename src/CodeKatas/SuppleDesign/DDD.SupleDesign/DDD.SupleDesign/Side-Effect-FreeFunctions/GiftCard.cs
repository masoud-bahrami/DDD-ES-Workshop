namespace DDD.SuppleDesign.Side_Effect_FreeFunctions;


// aggregate
public class GiftCard
{
    public string Owner { get; set; }
    public Money Balance { get; set; }

    public GiftCard(string owner , Money initialBalance)
    {
        Owner = owner;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        Balance.Deposit(amount);
    }

    public void Withdraw(decimal amount)
    {
        Balance.Withdraw(amount);
    }

}
public class Money
{
    public decimal Balance { get; set; }
    
    private Money(decimal balance)
    {
        Balance = balance;
    }

    

    public Money Deposit(decimal amount)
    {
//        Balance += amount;
        return new Money(Balance + amount);
    }

    public Money Withdraw(decimal amount)
    {
        //Balance -= amount;

        return new Money(Balance - amount);
    }
}