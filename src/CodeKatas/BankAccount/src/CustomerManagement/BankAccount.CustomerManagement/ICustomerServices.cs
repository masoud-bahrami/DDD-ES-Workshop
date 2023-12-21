namespace BankAccount.CustomerManagement;

public interface ICustomerServices
{
    Task Register(RegisterCustomerCommand cmd);
    Task<List<CustomerViewModel>> GetAll();
}