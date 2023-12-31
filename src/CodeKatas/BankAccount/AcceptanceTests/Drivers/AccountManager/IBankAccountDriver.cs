namespace BankAccount.AcceptanceTests.Drivers.AccountManager;

public interface IBankAccountDriver
{
    Task OpenBank(string owner, decimal initialAmount);
    Task AssertThatOwnerHasAnAccountWithInitialBalance(string owner, decimal expectedAmount);
    
}