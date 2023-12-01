using Microsoft.EntityFrameworkCore;

namespace BankAccount.BankFees;

public class BankFeesDbContext : DbContext
{
    public DbSet<BankFees> BankFees { get; set; }

    public BankFeesDbContext(DbContextOptions<BankFeesDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)

    
    {
        modelBuilder.Entity<BankFees>()
            .Property<int>("ID")
            .ValueGeneratedOnAdd();

        modelBuilder
            .Entity<BankFees>()
            .HasKey("ID");

        base.OnModelCreating(modelBuilder);
    }
}