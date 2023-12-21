using BankAccount.CustomerManagement.Services;

namespace BankAccount.CustomerManagement.Domain;

public class Customer
{
    public long Id { get; private set; }
    public string NationalCode { get; set; }
    public DateTime BirthDate { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string CustomerId { get; set; }


    private Customer() { }
    public Customer(long id, RegisterCustomerCommand cmd, ICustomerIdDomainService service)
    {
        Id = id;
        FirstName = cmd.FirstName;
        LastName = cmd.LastName;
        BirthDate = cmd.BirthDate;
        NationalCode = cmd.NationalCode;
        CustomerId = service.NextId();
    }


}