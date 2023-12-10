namespace Zero.Dispatcher.Query;

public abstract class IWantToHandleQuery<TQuery, TResult>
            where TQuery : IAmAQuery
{
    public abstract Task<TResult> Handle<T>(T query) where T : IAmAQuery;

    public virtual Task<TResult> FallBack(TQuery query, Exception exception)
        => throw new FallBackMethodShouldBeImplementedByQueryHandlerException<TQuery, TResult>(query);
}