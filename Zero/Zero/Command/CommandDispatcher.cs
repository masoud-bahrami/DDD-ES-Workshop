namespace Zero.Dispatcher.Command;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Dispatch<T>(T command) where T : IsACommand
    {
        var commandHandler = _serviceProvider.GetService(typeof(IWantToHandleCommand<T>)) as IWantToHandleCommand<T>;

        if (commandHandler is null)
            throw new HandlerNotFoundOrRegisteredException<T>();

        await commandHandler.Handle(command);
    }
}