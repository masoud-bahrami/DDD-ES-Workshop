namespace Zero.Dispatcher.QueryPipeline;

public class RemoveSpaceAndNormalizeArabicCharactersOfStringFieldsOfQueryStage
    : IAmQueryHandlerStage
{

    public override async Task<TResult> RuneQuery<TQuery, TResult>(TQuery query)
    {
        // TODO
        return await Next?.RuneQuery<TQuery, TResult>(query)!;
    }
}