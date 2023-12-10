namespace Zero.Dispatcher.Command;

public abstract class IsACommand
{
    public string CorrelationId { get; private set; }

    public void SetCorrelationId(string correlationId)
    {
        CorrelationId = correlationId;
    }
}