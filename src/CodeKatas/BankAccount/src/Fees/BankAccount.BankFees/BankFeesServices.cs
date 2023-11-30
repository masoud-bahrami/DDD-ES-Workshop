using BankAccount.Infrastructure;

namespace BankAccount.BankFees;

public class BankFeesServices : IBankFeesServices
{
    public readonly BankAccountDbContext _dbContext;

    public BankFeesServices(BankAccountDbContext dbContext) 
        => _dbContext = dbContext;

    public async Task SetFees(SetBankFeesCommand cmd)
    {
        _dbContext.BankFees.Add(new Infrastructure.BankFees(cmd.SmsFees, cmd.Charges));

        await _dbContext.SaveChangesAsync();
        
    }
}