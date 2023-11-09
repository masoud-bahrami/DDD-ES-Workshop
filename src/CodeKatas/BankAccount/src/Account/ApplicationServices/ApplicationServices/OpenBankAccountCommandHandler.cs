using BankAccount.ApplicationServices.Dispatcher;
using BankAccount.Domain;

namespace BankAccount.ApplicationServices;

public class OpenBankAccountCommandHandler : IWantToHandleCommand<OpenBankAccountCommand>
{
    private readonly IAccountDomainService _accountDomainService;

    private readonly IAccountRepository _repository;
    // command
    // procedural
    // orchestration (use case => Open a new bank account)
    public OpenBankAccountCommandHandler(IAccountDomainService accountDomainService, IAccountRepository repository)
    {
        _accountDomainService = accountDomainService;
        _repository = repository;
    }

    public override async Task Handle(OpenBankAccountCommand command)
    {
        var accountId = "1";

        Account account = new Account(accountId, command.InitialAmount, _accountDomainService);

        await _repository.Store(account);
    }
}