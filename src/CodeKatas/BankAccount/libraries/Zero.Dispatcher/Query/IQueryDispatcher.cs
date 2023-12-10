namespace Zero.Dispatcher.Query;

public interface IQueryDispatcher
{
    Task<T1> RunQuery<T, T1>(T query) where T : IAmAQuery;
}

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;


    public async Task<T1> RunQuery<T, T1>(T query) where T : IAmAQuery
    {
        var queryHandler = _serviceProvider
            .GetService(typeof(IWantToHandleQuery<T, T1>));

        if (queryHandler is null)
            throw new QueryHandlerNotFoundException<T, T1>();

        return await ((IWantToHandleQuery<T, T1>)queryHandler).Handle(query);
    }
}

public class QueryHandlerNotFoundException
    <T, T1> : Exception
{
}