using Microsoft.EntityFrameworkCore;

namespace BankAccount.CustomerManagement;

public class CustomerDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options):base(options)
    {
        
    }
}