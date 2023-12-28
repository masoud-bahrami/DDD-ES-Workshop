using BankAccount.BankFees;
using BankAccount.Domain.Services;

namespace BankAccount.ApplicationServices;

public interface IBankFeesAcl
{
    Task<BankFeesViewModel> FetchFees();
}

public class BankFeesAcl : IBankFeesAcl
{

    //private readonly IBankFeesServices _bankFeesServices;

    //public BankFeesAcl(IBankFeesServices bankFeesServices)
    //{
    //    _bankFeesServices = bankFeesServices;
    //}

    public async Task<BankFeesViewModel> FetchFees()
    {
            return new BankFeesViewModel(0, 0);

        //return
        //    var bankFee = await _bankFeesServices.GetFees();

        //if (bankFee is null)
        //    return new BankFeesViewModel(0, 0);


        //return new BankFeesViewModel(bankFee.SmsFees, bankFee.Charges);
    }
}