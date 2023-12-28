using BankAccount.BankFees.DAL;

namespace BankAccount.BankFees.BAL
{
    public interface IBankFeesServices
    {
        Task SetFees(SetBankFeesCommand cmd);
        Task<BankFee> GetFees();
    }
}