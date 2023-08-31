using CodeKata.PortsAndAdapters.Application.DrivenPorts;

namespace CodeKata.AcceptanceTests;

public class NotificationServiceDrivenPortMock : INotificationServiceDrivenPort
{
    public string _expectedMobileNumber;
    public string _expectedMessage;

    private string _actualMobile;
    private string _actualMessage;

    public void Notify(string mobile, string message)
    {
        _actualMobile = mobile;
        _actualMessage = message;
    }

    public void Verify()
    {
        Assert.Equal(_expectedMessage, _actualMessage);
        Assert.Equal(_actualMobile, _expectedMobileNumber);
    }

    public static INotificationServiceDrivenPort WhichIExpectCallWithParameter(string expectedMobileNumber, string expectedMessage)
    {
        return new NotificationServiceDrivenPortMock
        {
            _expectedMobileNumber = expectedMobileNumber,
            _expectedMessage = expectedMessage
        };
    }

    
}