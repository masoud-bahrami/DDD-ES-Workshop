namespace BankAccount.Domain;

public class Money
{
    public decimal Amount { get; set; }
    public string Curency { get; set; }
    public static Money Rial(decimal amount)
            => new Money
            {
                Amount = amount,
                Curency = "Rial"
            };

    public override bool Equals(object? obj)
    {
        var that = (Money)obj;

        return this.Amount == that.Amount
               && that.Curency == that.Curency;
    }
}