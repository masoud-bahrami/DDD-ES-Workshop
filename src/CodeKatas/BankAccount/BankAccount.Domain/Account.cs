namespace BankAccount.Domain.UnitTests;

public class Account
{
    public decimal Amount { get; set; }
    public Account(decimal initialAmount)
    {
        Amount = initialAmount;
    }


}