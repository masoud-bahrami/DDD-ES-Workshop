using Zero.Dispatcher.Command;

namespace Zero.Dispatcher.Tests.Command;

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