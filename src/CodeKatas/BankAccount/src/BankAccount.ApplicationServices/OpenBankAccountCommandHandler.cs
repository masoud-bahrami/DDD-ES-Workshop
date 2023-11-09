using BankAccount.ApplicationServices.Dispatcher;
using BankAccount.Domain;

namespace BankAccount.ApplicationServices;

public class OpenBankAccountCommandHandler : IWantToHandleCommand<OpenBankAccountCommand>
{
    private IAccountDomainService _accountDomainService;

    // command
    // procedural
    // orchestration (use case => Open a new bank account)
    public OpenBankAccountCommandHandler(IAccountDomainService accountDomainService)
    {
        _accountDomainService = accountDomainService;
    }

    public override Task Handle(OpenBankAccountCommand command)
    {
        var accountId = "1";

        Account account = new Account(accountId, command.InitialAmount, _accountDomainService);
        

        return Task.CompletedTask;
    }
}