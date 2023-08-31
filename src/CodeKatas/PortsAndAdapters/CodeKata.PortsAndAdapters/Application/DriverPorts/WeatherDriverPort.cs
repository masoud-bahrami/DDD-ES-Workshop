using CodeKata.PortsAndAdapters.Application.DrivenPorts;

namespace CodeKata.PortsAndAdapters.Application.DriverPorts;

public class WeatherDriverPort : IWeatherDriverPort
{
    private INotificationServiceDrivenPort NotificationPort { get; }
    private readonly IRepositoryDrivenPort _repositoryDrivenPort;
    private Dictionary<string, List<string>> _subscribers = new();

    public WeatherDriverPort(IRepositoryDrivenPort repositoryDrivenPort,
        INotificationServiceDrivenPort notificationPort)
    {
        NotificationPort = notificationPort;
        _repositoryDrivenPort = repositoryDrivenPort;
    }

    public Task SetWheatherOf(string city, int degree)
    {

        _repositoryDrivenPort.Set(city, degree);

        if (_subscribers.TryGetValue(city, out var subscribers))
        {
            foreach (var mobile in subscribers)
            {
                NotificationPort.Notify(mobile, $"The latest tehran weather is {degree}");
            }
        }

        return Task.CompletedTask;
    }

    public Task<int> InqueryWeatherOf(string city)
    {
        return Task.FromResult(_repositoryDrivenPort.InqueryWeatherOf(city));
    }

    public void Subscribe(string city, string mobile)
    {
        _subscribers[city] = new List<string> { mobile };
    }
}