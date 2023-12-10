using System.Reflection;
using Zero.Dispatcher.Command;

namespace Zero.Dispatcher.CommandPipeline.Stages
{
    public class HandlingCommandStage : IAmAPipelineStage
    {
        private readonly IServiceProvider _serviceProvider;

        public HandlingCommandStage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task Process<T>(T command, StageContext context)
        {
            var commandHandler = ResolveCommandHandlerOf<T>();

            if (commandHandler is null)
                throw new HandlerNotFoundOrRegisteredException<T>();
            
            try
            {
                await commandHandler.Handle(command);

                var queuedEvents = commandHandler.GetQueuedEvents();
                
                context.QueueDomainEvents(queuedEvents);
            }
            catch (Exception e)
            {
                if (ItNeedsToBeRetried(commandHandler, e))
                {
                    await commandHandler.Handle(command);
                }
                else
                {
                    throw e;
                }
            }


        }

        private IWantToHandleCommand<T>? ResolveCommandHandlerOf<T>()where T :IsACommand 
            => _serviceProvider.GetService(typeof(IWantToHandleCommand<T>)) as IWantToHandleCommand<T>;

        private static bool ItNeedsToBeRetried<T>(IWantToHandleCommand<T> commandHandler, Exception e) 
            where T : IsACommand
        {
            var retryAttribute = GetRetryAttributeOf(HandleMethodOf(commandHandler));
            
            return retryAttribute is not null && e.GetType() == retryAttribute.ExceptionType;


            MethodInfo? HandleMethodOf<T>(IWantToHandleCommand<T> commandHandler) where T : IsACommand
            {
                return commandHandler.GetType().GetMethods().FirstOrDefault(a => a.Name == "RuneQuery");
            }
            RetryAttribute? GetRetryAttributeOf(MethodInfo handleMethodOf)
            {
                var retryAttributeOf = handleMethodOf.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(RetryAttribute));
                return retryAttributeOf as RetryAttribute;
            }
        }

        
    }
}