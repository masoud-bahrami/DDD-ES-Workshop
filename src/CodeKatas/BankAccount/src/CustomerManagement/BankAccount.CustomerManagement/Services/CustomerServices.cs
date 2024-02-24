using BankAccount.CustomerManagement.DbContext;
using BankAccount.CustomerManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.CustomerManagement.Services;

public class CustomerServices : ICustomerServices
{
    private readonly CustomerDbContext _dbContext;
    private readonly ICustomerIdDomainService _customerIdDomainService;
    public CustomerServices(CustomerDbContext dbContext, ICustomerIdDomainService customerIdDomainService)
    {
        _dbContext = dbContext;
        _customerIdDomainService = customerIdDomainService;
    }

    public async Task Register(RegisterCustomerCommand cmd)
    {
        var id = 1111;
        var customer = new Customer(id, cmd, _customerIdDomainService);
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<CustomerViewModel>> GetAll()
    {
        return await
            _dbContext.Customers.AsNoTracking()
                .Select(a => new CustomerViewModel
                {
                    CustomerId = a.CustomerId
                }).ToListAsync();
    }
}