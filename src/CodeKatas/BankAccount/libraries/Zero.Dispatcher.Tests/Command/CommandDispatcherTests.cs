using Microsoft.Extensions.DependencyInjection;
using Zero.Dispatcher.Command;

namespace Zero.Dispatcher.Tests.Command
{
    public class CommandDispatcherTests
    {
        [Fact]
        // Sut(Action)  context assertion 
        public void dispatchingACommand_handlerIsNotRegistered_willThrowHandlerNotFoundOrRegisteredException()
        {
            ICommandDispatcher commandDispatcher = new CommandDispatcher(new ServiceCollection().BuildServiceProvider());

            var action = async () => await commandDispatcher.Dispatch(new FakeCommand());

            Assert.ThrowsAsync<HandlerNotFoundOrRegisteredException<FakeCommand>>(action);
        }


        [Fact]
        public async Task Will_Successfully_Dispatched_A_Command_To_Its_Handler()
        {
            var fakeCommand = new FakeCommand();

            var fakeCommandHandler = new FakeCommandHandler();

            // IoC
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IWantToHandleCommand<FakeCommand>>(sp => fakeCommandHandler);

            // Resolver
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            ICommandDispatcher _dispatcher = new CommandDispatcher(serviceProvider);

            await _dispatcher.Dispatch(fakeCommand);

            fakeCommandHandler.Verify();
        }
    }
}