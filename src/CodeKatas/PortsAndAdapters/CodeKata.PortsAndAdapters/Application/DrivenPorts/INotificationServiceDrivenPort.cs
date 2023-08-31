namespace CodeKata.PortsAndAdapters.Application.DrivenPorts;

public interface INotificationServiceDrivenPort
{
    void Notify(string mobile, string message);
}