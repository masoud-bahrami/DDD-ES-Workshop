
using Microsoft.EntityFrameworkCore;

namespace BankAccount.BankFees;

public class BankFeesServices : IBankFeesServices
{
    public readonly BankFeesDbContext _dbContext;

    public BankFeesServices(BankFeesDbContext dbContext) 
        => _dbContext = dbContext;

    public async Task SetFees(SetBankFeesCommand cmd)
    {
        _dbContext.BankFees.Add(new BankFees(cmd.SmsFees, cmd.Charges));

        await _dbContext.SaveChangesAsync();
        
    }

    public Task<BankFees> GetFees() 
        => _dbContext.BankFees.FirstOrDefaultAsync();
}