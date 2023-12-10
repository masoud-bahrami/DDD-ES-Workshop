namespace Zero.Dispatcher.CommandPipeline.Stages
{
    public class CorrelationIdStage : IAmAPipelineStage
    {

        public override async Task Process<T>(T command, StageContext context)
        {
            command.SetCorrelationId(Guid.NewGuid().ToString());

        }
    }
}