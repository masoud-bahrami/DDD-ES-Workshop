using Zero.Dispatcher.Command;

namespace Zero.Dispatcher.CommandPipeline
{
    public abstract class IAmAPipelineStage
    {

        public abstract Task Process<T>(T command, StageContext context)
            where T : IsACommand;
    }
}