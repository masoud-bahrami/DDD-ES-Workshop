using Zero.Domain;

namespace BankAccount.Domain.Accounts;

// json, bson
// homoiconic (LisP, Clojure) => code as data
//  var array = [1 , 2 , 3]
// process (array) => [1 , 2 , 3]

public class Transactions
{
    private readonly Queue<Transaction> _transactions;
    public static Transactions Init() => new();

    private Transactions()
        => _transactions = new Queue<Transaction>();

    public void Add(Transaction transaction)
        => _transactions.Enqueue(transaction);

    public void Add(IEnumerable<Transaction> transactions)
    {
        foreach (var tr in transactions)
        {
            Add(tr);
        }
    }

    public Money Balance()
    {
        var depositSummation = _transactions.Where(t => t.Type == TransactionType.Deposit)
            .Sum(t => t.Money.Amount);

        var withdrawalSummation = _transactions.Where(t => t.Type == TransactionType.Withdrawal)
            .Sum(t => t.Money.Amount);

        return Money.Rial(depositSummation - withdrawalSummation);
    }
}