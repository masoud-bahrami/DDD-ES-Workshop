using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ICommandDispatcher commandDispatcher;

    public AccountController(ICommandDispatcher commandDispatcher)
    {
        this.commandDispatcher = commandDispatcher;
    }

    [HttpPost("deposit/{amount}")]
    public async Task<IActionResult> Deposit(decimal amount)
    {
        var depositCommand = new DepositCommand(amount);
        await commandDispatcher.Dispatch(depositCommand);

        return Ok();
    }
}

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
}

public abstract class ICommand
{

}
public class DepositCommand : ICommand
{
    public decimal Amount { get; }

    public DepositCommand(decimal amount)
    {
        Amount = amount;
    }
}

public class DepositCommandHandler : ICommandHandler<DepositCommand>
{
    private IAccountRepository accountRepository;

    public DepositCommandHandler(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    public override async Task Handle(DepositCommand command)
    {
        var account = new Account("1", command);

        await accountRepository.Save(account);
    }
}

public interface IAccountRepository
{
    Task Save(Account account);
}

public class MyDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
    {
         this.Database.EnsureCreated();
        this.Database.Migrate();

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasKey(k => k.AccountId);

        base.OnModelCreating(modelBuilder);
    }
}

class AccountRepository : IAccountRepository
{
    private MyDbContext dbContext;

    public AccountRepository(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


    public async Task Save(Account account)
    {
        await dbContext.Set<Account>().AddAsync(account);
        await dbContext.SaveChangesAsync();
    }
}

public class Account
{
    public readonly string AccountId;
    public decimal Balance { get; set; }
    private Account(){}

    public Account(string accountId, DepositCommand command)
    {
        AccountId = accountId;
        Balance += command.Amount;
    }

    
}

public class AccountId
{
    public string Value;
    public AccountId(string value)
    {
        Value = value;
        
    }
}