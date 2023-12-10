using System.Reflection;
using Zero.Dispatcher.Query;

namespace Zero.Dispatcher.QueryPipeline;

public class PipelineQueryDispatcher : IAmQueryHandlerStage
{
    private readonly IServiceProvider _serviceProvider;


    public PipelineQueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public override async Task<TResult> RuneQuery<TQuery, TResult>(TQuery query)
    {
        var service = _serviceProvider
            .GetService(typeof(IWantToHandleQuery<TQuery, TResult>));

        if (service is null)
            throw new QueryHandlerNotFoundException<TQuery, TResult>();

        var queryHandler = ((IWantToHandleQuery<TQuery, TResult>)service);

        try
        {
            return await queryHandler.Handle(query);
        }



        catch (Exception e)
        {
            var firstOrDefault = IsHandlerImplementedFallback(queryHandler);

            if (firstOrDefault is not null)
                return await queryHandler.FallBack(query, e);

            throw;
        }
    }

    private static MethodInfo IsHandlerImplementedFallback<TQuery, TResult>(IWantToHandleQuery<TQuery, TResult> queryHandler)
        where TQuery : IAmAQuery
    {
        return queryHandler.GetType().GetMethods().FirstOrDefault(a => a.Name == nameof(queryHandler.FallBack));
    }
}
