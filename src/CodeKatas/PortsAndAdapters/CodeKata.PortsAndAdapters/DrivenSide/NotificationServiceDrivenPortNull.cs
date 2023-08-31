using CodeKata.PortsAndAdapters.Application.DrivenPorts;

namespace CodeKata.PortsAndAdapters.DrivenSide;

public class NotificationServiceDrivenPortNull : INotificationServiceDrivenPort
{
    public void Notify(string mobile, string message)
    {

    }
    
}