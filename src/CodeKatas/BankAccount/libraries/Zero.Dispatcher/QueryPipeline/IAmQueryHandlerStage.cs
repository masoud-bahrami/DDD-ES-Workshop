using Zero.Dispatcher.Query;

namespace Zero.Dispatcher.QueryPipeline;

public abstract class IAmQueryHandlerStage
{
    public IAmQueryHandlerStage? Next;
    public abstract Task<TResult> RuneQuery<TQuery, TResult>(TQuery query) where TQuery : IAmAQuery;
}