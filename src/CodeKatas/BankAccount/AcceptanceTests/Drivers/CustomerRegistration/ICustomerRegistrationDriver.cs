namespace BankAccount.AcceptanceTests.Drivers.CustomerRegistration;

public interface ICustomerRegistrationDriver
{
    Task Register(string customerName, Table table);
    Task AssertCustomerId(string customerId);
}