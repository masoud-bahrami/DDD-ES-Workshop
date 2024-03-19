namespace BankAccount.Domain.Accounts.Memento;

public class AccountMemento
{
    public AccountMemento()
    {
        
    }
    public AccountMemento(string id, Transactions transactions, Bank openedIn)
    {
        Id = id;
        Transactions = transactions;
        OpenedIn = openedIn;
    }

    public string Id { get; set; }
    public Transactions Transactions { get; set; } = Transactions.Init();

    public Bank OpenedIn { get;set ; }

    public decimal Balance() => Transactions.Balance().Amount;
}