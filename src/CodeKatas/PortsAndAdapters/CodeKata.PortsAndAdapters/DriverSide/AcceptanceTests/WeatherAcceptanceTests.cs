using CodeKata.PortsAndAdapters.Application.DrivenPorts;
using CodeKata.PortsAndAdapters.Application.DriverPorts;
using CodeKata.PortsAndAdapters.DrivenSide;

namespace CodeKata.AcceptanceTests
{
    // Acceptance test
    // outside in

    public class WeatherAcceptanceTests
    {

        // Adapter

        [Theory]
        [InlineData(38)]
        [InlineData(40)]
        [InlineData(54)]
        [InlineData(20)]
        // Data-Driven tests
        public async Task set_weather_of_a_city_manually(int expectedDegree)
        {
            var weatherDriverPort = CreateWeatherDriverPort();

            var city = "Tehran";
            await weatherDriverPort.SetWheatherOf(city, expectedDegree);

            // triggering the driver 
            int actualWeather = await weatherDriverPort.InqueryWeatherOf(city);

            Assert.Equal(expectedDegree, actualWeather);
        }

      


        [Fact]
        public void weather_subscribtion()
        {
            var expectedWeather = 40;
            var mobile = "09123456789";

            var notificationServiceDrivenPort = NotificationServiceDrivenPortMock
                            .WhichIExpectCallWithParameter(expectedMobileNumber:mobile, 
                                expectedMessage:$"The latest tehran weather is {expectedWeather}");
            // sut
            // assert
            //  SUT
            IWeatherDriverPort weatherDriverPort = CreateWeatherDriverPort(notificationServiceDrivenPort);

            weatherDriverPort.Subscribe("tehran", mobile);

            weatherDriverPort.SetWheatherOf("tehran", expectedWeather);
            
            // Mock
            // Assert

            ((NotificationServiceDrivenPortMock)notificationServiceDrivenPort).Verify();
        }

        private static IWeatherDriverPort CreateWeatherDriverPort(INotificationServiceDrivenPort? notificationServiceDrivenPort = null)
        {

            if (notificationServiceDrivenPort == null)
            {
                notificationServiceDrivenPort = new NotificationServiceDrivenPortNull();
            }

            IRepositoryDrivenPort repositoryDrivenPort
                = new InMemoryRepositoryDriverPort();
            // triggering the driver 
            IWeatherDriverPort weatherDriverPort = new WeatherDriverPort(repositoryDrivenPort,
                notificationServiceDrivenPort);
            return weatherDriverPort;
        }
    }
}