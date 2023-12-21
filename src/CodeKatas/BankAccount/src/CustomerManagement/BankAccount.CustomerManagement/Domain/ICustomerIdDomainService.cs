using System.Net.Http.Headers;

namespace BankAccount.CustomerManagement.Domain
{
    public interface ICustomerIdDomainService
    {
        string NextId();
    }

    public class CustomerIdDomainService : ICustomerIdDomainService
    {
        public string NextId() 
            => "1234-4321-2532-2533";
    }
}
