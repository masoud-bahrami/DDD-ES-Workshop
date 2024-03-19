namespace Zero.EventSourcing;

public interface ICatchUpSubscriber : ISubscriber
{
    void LiveProcessingStarted();
}

public interface ISubscriber
{
    string Name { get; }
    void AnEventAppended(EventViewModel eventViewModel);
}