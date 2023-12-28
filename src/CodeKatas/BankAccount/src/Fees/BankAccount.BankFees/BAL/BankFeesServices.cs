using BankAccount.BankFees.DAL;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.BankFees.BAL;

public class BankFeesServices : IBankFeesServices
{
    public readonly BankFeesDbContext _dbContext;

    public BankFeesServices(BankFeesDbContext dbContext)
        => _dbContext = dbContext;

    public async Task SetFees(SetBankFeesCommand cmd)
    {
        _dbContext.BankFees.Add(new DAL.BankFee(cmd.SmsFees, cmd.Charges));

        await _dbContext.SaveChangesAsync();

    }

    public Task<DAL.BankFee> GetFees()
        => _dbContext.BankFees.FirstOrDefaultAsync();
}