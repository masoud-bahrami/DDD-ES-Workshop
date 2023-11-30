using System.Text.Json;
using System.Text.Json.Serialization;
using BankAccount.Domain.Accounts.Memento;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Infrastructure;

public class BankAccountDbContext : DbContext
{
    public DbSet<AccountMemento> Accounts { get; set; }
    public DbSet<BankFees> BankFees { get; set; }

    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountMemento>()
            .Property(p => p.Transactions)
            .HasConversion<TransactionsConverter>();
        
        modelBuilder.Entity<BankFees>()
            .Property<int>("ID")
            .ValueGeneratedOnAdd();

        modelBuilder
            .Entity<BankFees>()
            .HasKey("ID");

        base.OnModelCreating(modelBuilder);
    }
}