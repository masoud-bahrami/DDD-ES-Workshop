using BankAccount.CustomerManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.CustomerManagement.DbContext;

public class CustomerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {

    }
}