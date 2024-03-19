using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero.Dispatcher.CommandPipeline;
using Zero.Domain;
using Zero.EventSourcing.Projection;

namespace Zero.EventSourcing.DispatcherPipeline
{
    public class ProjectDomainEventsStage : IAmAPipelineStage
    {
        private readonly IProjectorsLedger _ledger;
        private readonly IServiceProvider _resolver;

        public ProjectDomainEventsStage(IProjectorsLedger ledger, IServiceProvider resolver)
        {
            _ledger = ledger;
            _resolver = resolver;
        }

        public override async Task Process<T>(T command, StageContext context)
        {
            var domainEvents = context.GetDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                await Project(domainEvent);
            }
        }

        private async Task Project(KeyValuePair<IsAnIdentity, QueuedEvents> domainEvent)
        {
            foreach (var domainEventsValue in domainEvent.Value.Events)
            {
                var projectorTypes = WhoAreInterestedIn(domainEventsValue.GetType().AssemblyQualifiedName);

                foreach (var projector in projectorTypes.Select(projectorType => _resolver.GetService(projectorType)))
                {
                    await ProjectEvent(projector, domainEventsValue);
                }
            }
        }

        private IEnumerable<Type> WhoAreInterestedIn(string eventAssemblyQualifiedName)
        {
            var whoAreInterestedIn = _ledger.WhoAreInterestedIn(Type.GetType(eventAssemblyQualifiedName));
            
            return whoAreInterestedIn;
        }
        
        private async Task ProjectEvent(object projector, IsADomainEvent domainEventsValue)
        {
            await ((ImAProjector)projector).Process(domainEventsValue);
        }
        
    }
}