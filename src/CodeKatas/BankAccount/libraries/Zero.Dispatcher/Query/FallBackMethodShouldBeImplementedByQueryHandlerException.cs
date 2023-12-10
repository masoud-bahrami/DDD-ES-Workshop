namespace Zero.Dispatcher.Query;

public class FallBackMethodShouldBeImplementedByQueryHandlerException<TQuery, TResult> : Exception
    where TQuery : IAmAQuery
{
    public IAmAQuery Query { get; }

    public FallBackMethodShouldBeImplementedByQueryHandlerException(IAmAQuery query)
    {
        Query = query;
    }
}