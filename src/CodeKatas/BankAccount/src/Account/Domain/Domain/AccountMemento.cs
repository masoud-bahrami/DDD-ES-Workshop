namespace BankAccount.Domain;

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