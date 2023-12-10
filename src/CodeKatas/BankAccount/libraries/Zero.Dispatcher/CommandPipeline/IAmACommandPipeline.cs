using Zero.Dispatcher.Command;
using Zero.Domain;

namespace Zero.Dispatcher.CommandPipeline
{
    public interface IAmACommandPipeline
    {
        void AddStage(IAmAPipelineStage stage);
        Task Process<T>(T command) where T : IsACommand;
        void IWantToListenTo<T>(Action<IsADomainEvent, long, string> action) where T : IsADomainEvent;
        Queue<IAmAPipelineStage> Stages();
        bool HasAnyStages();
    }

    public class CommandPipeline : IAmACommandPipeline
    {
        private readonly Queue<IAmAPipelineStage> _stages;
        private readonly StageContext _context = new();

        public CommandPipeline()
            => _stages = new Queue<IAmAPipelineStage>();

        public void AddStage(IAmAPipelineStage stage)
            => _stages.Enqueue(stage);

        public async Task Process<T>(T command)
            where T : IsACommand
        {
            var starterStage = _stages.First();

            if (starterStage == null)
            {
                throw new StarterStageIsEmptyOrNullException("starter stage is empty");
            }
            
            foreach (var amAPipelineStage in _stages)
            {
              await amAPipelineStage.Process(command, _context);
            };
        }

        public void IWantToListenTo<T>(Action<IsADomainEvent, long, string> action)
            where T : IsADomainEvent =>
            _context.AddEventListeners<T>(action);

        public Queue<IAmAPipelineStage> Stages()
            => _stages;

        public bool HasAnyStages()
            => _stages != null && _stages.Any();

        public class StarterStageIsEmptyOrNullException : Exception
        {
            public StarterStageIsEmptyOrNullException(string starterStageIsEmpty)
                : base(starterStageIsEmpty)
            {
            }
        }
    }
}