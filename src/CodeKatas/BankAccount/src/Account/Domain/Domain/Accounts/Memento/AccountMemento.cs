namespace BankAccount.Domain.Accounts.Memento;

public class AccountMemento
{
    public AccountMemento(string id, decimal amount)
    {
        Id = id;
        Amount = amount;
    }

    public string Id { get; set; }
    public decimal Amount { get; set; }
}