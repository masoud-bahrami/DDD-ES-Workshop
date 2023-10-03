namespace DDD.SuppleDesign.Assertions;

public class CashAccount
{
    public decimal Balance { get; private set; }

    public void Increase(decimal balance)
    {
        Balance += balance;
    }

    public decimal Decrease(decimal balance)
    {
        Balance -= balance;
        return balance;
    }
}