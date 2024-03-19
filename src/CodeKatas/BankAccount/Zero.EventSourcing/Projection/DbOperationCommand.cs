namespace Zero.EventSourcing.Projection;

public abstract class DbOperationCommand
{
    public abstract Task Execute();
}