using System.Runtime.CompilerServices;
using FluentAssertions;

namespace Zero.Domain.Tests
{
    public class InTermsOfAggregateRoot<T, TId>
      where T : EventSourcedEntity<TId>
      where TId : IsAnIdentity
    {
        private T _aggregate;

        public static InTermsOfAggregateRoot<T, TId> IfICreate(Func<T> func) => new InTermsOfAggregateRoot<T, TId>()
        {
            _aggregate = func()
        };

        public InTermsOfAggregateRoot<T, TId> ThenIWillExpectTheseEvents(
          params IsADomainEvent[] events)
        {
            List<IsADomainEvent> queuedEvents = _aggregate.GetQueuedEvents();
            queuedEvents.Count.Should().Be(events.Length, "");
            queuedEvents.SequenceEqual((IEnumerable<IsADomainEvent>)events).Should().BeTrue("");
            return this;
        }

        public void And(Func<T, bool> action) => action(_aggregate).Should().BeTrue("");

        public void ThenIWillExpect(Func<T, bool> action) => And(action);

        public void And(params Action<T>[] actions)
        {
            List<Exception> exceptionList = new List<Exception>();
            foreach (Action<T> action in actions)
            {
                try
                {
                    action(_aggregate);
                }
                catch (Exception ex)
                {
                    exceptionList.Add(ex);
                }
            }
            if (exceptionList.Any())
                throw new AggregateException(exceptionList);
        }

        public static InTermsOfAggregateRoot<T, TId> IfIApplied(
          params IsADomainEvent[] events)
        {
            if (events == null || !events.Any())
                throw new Exception("Events can not be null or empty array!");
            object instance;
            try
            {
                instance = Activator.CreateInstance(typeof(T), (object)events);
            }
            catch (MissingMethodException ex)
            {
                Console.WriteLine(ex);
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 2);
                interpolatedStringHandler.AppendLiteral("the ");
                interpolatedStringHandler.AppendFormatted(typeof(T));
                interpolatedStringHandler.AppendLiteral(" must have one public constructor with one parameters : ");
                interpolatedStringHandler.AppendFormatted(events.GetType());
                interpolatedStringHandler.AppendLiteral(" ");
                throw new Exception(interpolatedStringHandler.ToStringAndClear());
            }
            return new InTermsOfAggregateRoot<T, TId>()
            {
                _aggregate = (T)instance
            };
        }

        public InTermsOfAggregateRoot<T, TId> WhenICall(Action<T> func)
        {
            func(_aggregate);
            return this;
        }

        public void ThenIWillExpect(params Action<T>[] actions) => And(actions);
    }
}
