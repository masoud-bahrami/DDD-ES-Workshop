
namespace Zero.Dispatcher.CommandPipeline
{
    public class HeyPipeline
    {
        public static HeyPipeline IWant() => new();
        public HeyPipeline ToDefineAPipeline()
        {
            return new HeyPipeline();
        }
        public PipelineBuilder WithStarterStage(IAmAPipelineStage stage)
        {
            return new PipelineBuilder(stage);
        }

        public class PipelineBuilder
        {
            private readonly IAmACommandPipeline _quantumPipeline;
            
            public PipelineBuilder(IAmAPipelineStage starterLastStage)
            {
                _quantumPipeline = new CommandPipeline();

                if(starterLastStage is null)
                    throw new ArgumentNullException("starterLastStage");

                _quantumPipeline.AddStage(starterLastStage);
            }

            public PipelineBuilder WithSuccessor(IAmAPipelineStage newStage)
            {
                _quantumPipeline.AddStage(newStage);
             
                return this;
            }

            public IAmACommandPipeline ThankYou()
            {
                return _quantumPipeline;
            }
            
        }
    }


}