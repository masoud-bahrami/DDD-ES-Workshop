using CodeKata.PortsAndAdapters.Application.DrivenPorts;

namespace CodeKata.PortsAndAdapters.DrivenSide;

public class InMemoryRepositoryDriverPort : IRepositoryDrivenPort
{
    private readonly Dictionary<string, int> _cityWeathers = new();

    public void Set(string city, int degree)
    {
        _cityWeathers[city] = degree;
    }

    public int InqueryWeatherOf(string city)
    {
        return _cityWeathers[city];
    }
}