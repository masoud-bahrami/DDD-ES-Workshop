using System.Text.Json;
using System.Text.Json.Serialization;
using BankAccount.Domain.Accounts.Memento;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Infrastructure;

// Accounts
// | Id | Transactions|
// | 1  | [{} , {}] |

 
public class BankAccountDbContext : DbContext
{
    public DbSet<AccountMemento> Accounts { get; set; }
    

    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountMemento>()
            .Property(p => p.Transactions)
            .HasConversion<TransactionsConverter>();
        
        base.OnModelCreating(modelBuilder);
    }
}