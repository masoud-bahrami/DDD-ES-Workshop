using BankAccount.Domain.Accounts.Memento;
using Microsoft.EntityFrameworkCore;
using Zero.DataBase;

namespace BankAccount.Infrastructure;

// Accounts
// | Id | Transactions|
// | 1  | [{} , {}] |

 
public class BankAccountDbContext : ZeroDbContext
{
    public DbSet<AccountMemento> Accounts { get; set; }
    

    public BankAccountDbContext(DbContextOptions<ZeroDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountMemento>()
            .Property(p => p.Transactions)
            .HasConversion<TransactionsConverter>();

        modelBuilder.Entity<AccountMemento>()
            .OwnsOne(typeof(Domain.Accounts.Bank), "OpenedIn");

        // OpenId_Name | OpenId_Branch
        base.OnModelCreating(modelBuilder);
    }
}