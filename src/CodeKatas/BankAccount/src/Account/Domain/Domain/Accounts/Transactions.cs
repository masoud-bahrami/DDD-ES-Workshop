namespace BankAccount.Domain.Accounts;

public class Transactions
{
    private readonly Queue<Transaction> _transactions;
    public static Transactions Init() => new();

    private Transactions() 
        => _transactions = new Queue<Transaction>();

    public void Add(Transaction transaction) 
        => _transactions.Enqueue(transaction);

    public Money Balance()
    {
        var depositSummation = _transactions.Where(t => t.Type == TransactionType.Deposit)
            .Sum(t=>t.Money.Amount);

        var withdrawalSummation = _transactions.Where(t => t.Type == TransactionType.Withdrawal)
            .Sum(t=>t.Money.Amount);

        return Money.Rial(depositSummation - withdrawalSummation);
    }
}