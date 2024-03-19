namespace Zero.EventSourcing.Projection;

public interface IProjectorsLedger
{
    List<Type> WhoAreInterestedIn(Type type);
}