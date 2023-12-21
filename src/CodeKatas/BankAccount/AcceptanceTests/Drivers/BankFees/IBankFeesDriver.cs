namespace BankAccount.AcceptanceTests.Drivers.BankFees;

public interface IBankFeesDriver
{
    Task SetFees(Table amount);
}