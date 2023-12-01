using Microsoft.Extensions.DependencyInjection;
using Zero.Dispatcher.Query;

namespace Zero.Dispatcher.Tests.Query;

public class QueryDispatcherTests
{
    [Fact]
    public async Task queryShouldDispatchedToItsHandler()
    {
        var fakeQuery = new FakeQuery();
        var fakeQueryHandler = FakeQueryHandler
            .WithCalledOnce();

        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IWantToHandleQuery<FakeQuery, bool> , FakeQueryHandler>
            (sp=>fakeQueryHandler);

        IServiceProvider sp = serviceCollection.BuildServiceProvider();

        IQueryDispatcher queryDispatcher = new QueryDispatcher(sp);

        await queryDispatcher.RunQuery<FakeQuery, bool>(fakeQuery);

        fakeQueryHandler.Verify();
    }

}

public class FakeQueryHandler : IWantToHandleQuery<FakeQuery, bool>
{
    private int _actualNumber;
    private int _expectedNumber { get;  set; }

    public static FakeQueryHandler WithCalledOnce()
    {
        return new FakeQueryHandler {_expectedNumber = 1};
    }

   


    public override Task<bool> Handle<T>(T query)
    {
        _actualNumber++;
        return Task.FromResult(true);
    }


    public void Verify()
    {
        Assert.Equal(_actualNumber, _expectedNumber);
    }
}

public class FakeQuery : IQuery
{
}