using Zero.Dispatcher.Command;

namespace BankAccount.ApplicationServices;

public class DefineActivityCommandHandler : IWantToHandleCommand<DefineActivityCommand>
{
    public override Task Handle(DefineActivityCommand command)
    {
        throw new NotImplementedException();
    }
}