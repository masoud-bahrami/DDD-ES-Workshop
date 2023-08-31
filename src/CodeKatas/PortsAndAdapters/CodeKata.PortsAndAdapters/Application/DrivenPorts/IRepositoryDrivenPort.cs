namespace CodeKata.PortsAndAdapters.Application.DrivenPorts;

public interface IRepositoryDrivenPort
{
    void Set(string city, int degree);
    int InqueryWeatherOf(string city);
}