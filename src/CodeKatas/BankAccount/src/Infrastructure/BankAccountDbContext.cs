using BankAccount.Domain.Accounts.Memento;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Infrastructure;

public class BankAccountDbContext : DbContext
{
    public DbSet<AccountMemento> Accounts { get; set; }

    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options)
            : base(options)
    {
    }
}