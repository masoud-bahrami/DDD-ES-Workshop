namespace Zero.Dispatcher.CommandPipeline.Stages
{
    public class CallEventListenersStage : IAmAPipelineStage
    {
        public override async Task Process<TContext>(TContext command, StageContext context)
        {
            foreach (var x in context.GetDomainEvents())
            {
                foreach (var item in x.Value.Events)
                {
                    var type = item.GetType();

                    if (!context.DomainEventListeners.TryGetValue(type, out var listeners)) continue;

                    foreach (var listener in listeners)
                    {
                        listener.Invoke(item, x.Value.ExpectedVersion, x.Key.ToString());
                    }
                }
            }
        }
    }
}