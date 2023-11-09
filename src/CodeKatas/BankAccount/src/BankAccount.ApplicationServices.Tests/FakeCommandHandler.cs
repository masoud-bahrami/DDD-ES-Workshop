using BankAccount.ApplicationServices.Dispatcher;

namespace BankAccount.ApplicationServices.Tests;

public class FakeCommand : IsACommand
{
}

public class FakeCommandHandler : IWantToHandleCommand<FakeCommand>
{
    private bool _isCalled;

    public void Verify()
    {
        Assert.Equal(true, _isCalled);
    }

    public override Task Handle(FakeCommand command)
    {
        _isCalled = true;
        return Task.CompletedTask;
    }
}