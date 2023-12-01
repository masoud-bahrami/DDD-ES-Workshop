namespace BankAccount.BankFees
{
    public interface IBankFeesServices
    {
        Task SetFees(SetBankFeesCommand cmd);
        Task<BankFees> GetFees();
    }
}