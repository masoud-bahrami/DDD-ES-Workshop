namespace CodeKata.PortsAndAdapters.Application.DriverPorts;

public interface IWeatherDriverPort
{
    Task SetWheatherOf(string city, int degree);
    Task<int> InqueryWeatherOf(string city);
    void Subscribe(string city, string mobile);
}