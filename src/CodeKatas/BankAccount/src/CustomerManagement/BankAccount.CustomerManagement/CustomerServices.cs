using Microsoft.EntityFrameworkCore;

namespace BankAccount.CustomerManagement;

public class CustomerServices : ICustomerServices
{ 
    private readonly CustomerDbContext _dbContext;

    public CustomerServices(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Register(RegisterCustomerCommand cmd)
    {
        var customer = new Customer(2532, cmd);
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<CustomerViewModel>> GetAll()
    {
        return await 
            _dbContext.Customers.AsNoTracking()
                .Select(a => new CustomerViewModel
                {
                    CustomerId = "1234-4321-2532-2534"
                }).ToListAsync();
    }
}