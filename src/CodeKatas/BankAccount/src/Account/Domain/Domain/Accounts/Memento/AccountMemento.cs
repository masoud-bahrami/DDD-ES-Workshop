namespace BankAccount.Domain.Accounts.Memento;

public class AccountMemento
{
    public AccountMemento(string id, Transactions transactions)
    {
        Id = id;
        Transactions = transactions;
    }

    public string Id { get; set; }
    public Transactions Transactions { get;private set; }

    public decimal Balance() => Transactions.Balance().Amount;

}