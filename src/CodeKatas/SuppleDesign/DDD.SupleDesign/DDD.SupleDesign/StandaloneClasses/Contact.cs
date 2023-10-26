using System.Xml.Schema;

namespace DDD.SuppleDesign.StandaloneClasses;

public class Email
{
    public string Address { get; private set; }

    public Email(string address)
    {
        Guard(address);
        Address = address;
    }

    private void Guard(string address)
    {
        //TODO
    }

    public Email Update(string email)
    {
        return new Email(email);
    }
}

public class Contact
{
    public string PhoneNumber { get; set; }
    public string Tell { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}




public class Employee
{
    public string PhoneNumber { get; set; }
    public string Tell { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}

public class BuisnessParty
{
    public string PhoneNumber { get; set; }
    public string Tell { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}