namespace BankAccount.CustomerManagement;

public record RegisterCustomerCommand(string FirstName, string LastName,
    string NationalCode, DateTime BirthDate);