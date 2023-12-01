namespace Zero.Dispatcher.Query;

public abstract class IWantToHandleQuery<TQuery, TResult>
            where TQuery : IQuery
{
    public abstract Task<TResult> Handle<T>(T query) where T : IQuery;
}