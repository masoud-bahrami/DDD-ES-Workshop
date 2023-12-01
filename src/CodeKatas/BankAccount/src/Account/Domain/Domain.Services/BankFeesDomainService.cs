using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Services;

namespace BankAccount.Domain.Services;

public class BankFeesDomainService : IBankFeesDomainService
{

    private readonly BankFeesViewModel _viewModel;

    public BankFeesDomainService(BankFeesViewModel viewModel)
    {
        _viewModel = viewModel;
    }


    public IEnumerable<Transaction> CalculateFeesOfNewOpeningAccount()
    {
        yield return SmsFeesTransaction.New(Money.Rial(_viewModel.sms));
        yield return BankChargesTransaction.New(Money.Rial(_viewModel.charge));
    }
}

public record BankFeesViewModel(decimal sms, decimal charge) { }