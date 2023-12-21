namespace BankAccount.CustomerManagement.Services;

public interface ICustomerServices
{
    Task Register(RegisterCustomerCommand cmd);
    Task<List<CustomerViewModel>> GetAll();
}