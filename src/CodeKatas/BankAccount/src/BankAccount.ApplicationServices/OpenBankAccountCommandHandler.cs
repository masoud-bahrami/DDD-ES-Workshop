using BankAccount.ApplicationServices.Dispatcher;

namespace BankAccount.ApplicationServices;

public class OpenBankAccountCommandHandler : IWantToHandleCommand<OpenBankAccountCommand>
{
    // command
    // procedural
    // orchestration (use case => Open a new bank account)
    public override Task Handle<T>(T command)
    {
        // pre-conditions
        
        // 
        // Account(params);

        // store (repository)

        throw new NotImplementedException(nameof(OpenBankAccountCommandHandler));
    }
}