namespace BankAccount.CustomerManagement;

public class Customer
{
    public long Id { get; private set; }
    public string NationalCode { get; set; }
    public DateTime BirthDate { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }

    private Customer(){}
    public Customer(long id, RegisterCustomerCommand cmd)
    {
        Id = id;
        FirstName = cmd.FirstName;
        LastName= cmd.LastName;
        BirthDate = cmd.BirthDate;
        NationalCode = cmd.NationalCode;
    }

    
}