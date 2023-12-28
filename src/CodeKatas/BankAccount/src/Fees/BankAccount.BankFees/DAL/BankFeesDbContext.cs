using Microsoft.EntityFrameworkCore;

namespace BankAccount.BankFees.DAL;

public class BankFeesDbContext : DbContext
{
    public DbSet<BankFee> BankFees { get; set; }

    public BankFeesDbContext(DbContextOptions<BankFeesDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)


    {
        modelBuilder.Entity<BankFee>()
            .Property<int>("ID")
            .ValueGeneratedOnAdd();

        modelBuilder
            .Entity<BankFee>()
            .HasKey("ID");

        base.OnModelCreating(modelBuilder);
    }
}