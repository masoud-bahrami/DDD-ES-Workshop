namespace BankAccount.CustomerManagement.Services;

public record RegisterCustomerCommand(string FirstName, string LastName,
    string NationalCode, DateTime BirthDate);