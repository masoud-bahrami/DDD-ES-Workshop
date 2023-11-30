using BankAccount.Domain.Accounts;
using BankAccount.Domain.Accounts.Repository;
using BankAccount.Domain.Accounts.Services;
using Zero.Dispatcher.Command;

namespace BankAccount.ApplicationServices;

public class OpenBankAccountCommandHandler : IWantToHandleCommand<OpenBankAccountCommand>
{
    private readonly IAccountDomainService _accountDomainService;

    private readonly IAccountRepository _repository;
    private readonly IAccountIdGeneratorDomainService _accountIdGeneratorService;
    // command
    // procedural
    // orchestration (use case => Open a new bank account)
    public OpenBankAccountCommandHandler(IAccountDomainService accountDomainService, IAccountRepository repository, IAccountIdGeneratorDomainService accountIdGeneratorService)
    {
        _accountDomainService = accountDomainService;
        _repository = repository;
        _accountIdGeneratorService = accountIdGeneratorService;
    }

    public override async Task Handle(OpenBankAccountCommand command)
    {
        var account = new Account(_accountIdGeneratorService.CreateNewAccountId(), command.InitialAmount, _accountDomainService);

        await _repository.Store(account);
    }
}