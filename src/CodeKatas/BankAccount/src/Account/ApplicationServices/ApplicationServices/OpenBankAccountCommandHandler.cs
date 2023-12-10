using Bank.Account.Domain.Contracts.Commands;
using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Repository;
using BankAccount.Domain.Accounts.Services;
using BankAccount.Domain.Services;
using Zero.Dispatcher.Command;
using Zero.Dispatcher.CommandPipeline;

namespace BankAccount.ApplicationServices;

public class OpenBankAccountCommandHandler : IWantToHandleCommand<OpenBankAccountCommand>
{
    private readonly IAccountDomainService _accountDomainService;

    private readonly IAccountRepository _repository;
    private readonly IAccountIdGeneratorDomainService _accountIdGeneratorService;

    private readonly IBankFeesAcl _bankFeesAcl;
    // command
    // procedural
    // orchestration (use case => Open a new bank account)
    public OpenBankAccountCommandHandler(IAccountDomainService accountDomainService,
        IAccountRepository repository, IAccountIdGeneratorDomainService accountIdGeneratorService, IBankFeesAcl bankFeesAcl)
    {
        _accountDomainService = accountDomainService;
        _repository = repository;
        _accountIdGeneratorService = accountIdGeneratorService;
        _bankFeesAcl = bankFeesAcl;
    }

    [Retry(typeof(Exception))]
    public override async Task Handle(OpenBankAccountCommand command)
    {
        var account = new Account(_accountIdGeneratorService.CreateNewAccountId(), command.InitialAmount,
            _accountDomainService,
            await BankFeesDomainService());

        await _repository.Store(account);

        QueueDomainEvents(account.Identity, account.GetEvents());


        //command.Id = account.Identity.Id;
    }

    public async Task Compensate(OpeningAccountTransaction command)
    {
    }

    private async Task<IBankFeesDomainService> BankFeesDomainService()
    {
        return new BankFeesDomainService(await _bankFeesAcl.FetchFees());
    }

}