namespace Zero.Dispatcher.QueryPipeline;

public class HeyQueryDispatcherPipeline
{
    private IAmQueryHandlerStage _lastStage;
    private IAmQueryHandlerStage _firstStage;

    public static HeyQueryDispatcherPipeline CreateAQueryDispatcher()
    {
        return new HeyQueryDispatcherPipeline();
    }

    public HeyQueryDispatcherPipeline WithStartingStage(IAmQueryHandlerStage stage)
    {
        _firstStage = stage;
        _lastStage = stage;
        return this;
    }

    public HeyQueryDispatcherPipeline ProceedBy(IAmQueryHandlerStage stage)
    {
        if (_lastStage is not null)
            _lastStage.Next = stage;

        _lastStage = stage;
        return this;
    }

    public IAmQueryHandlerStage ThankYou()
    {
        return _firstStage;
    }
}