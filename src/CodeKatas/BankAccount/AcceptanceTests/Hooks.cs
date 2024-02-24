using BankAccount.AcceptanceTests.Drivers.AccountManager;
using BankAccount.AcceptanceTests.Drivers.Activities;
using BankAccount.AcceptanceTests.Drivers.BankFees;
using BankAccount.AcceptanceTests.Drivers.CustomerRegistration;
using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BankAccount.AcceptanceTests
{
    [Binding]
    public sealed class Hooks
    {
        private HttpClient _httpClient;
        ScenarioContext _scenarioContext;
        IObjectContainer _container;
        public Hooks(ScenarioContext scenarioContext, IObjectContainer container)
        {
            _scenarioContext = scenarioContext;
            _container = container;
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario()
        {
            var application = new WebApplicationFactory<CustomerManagement.Host.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                    });
                });


            _httpClient = application.CreateClient();

            _container.RegisterInstanceAs(_httpClient, typeof(HttpClient));

            _container.RegisterTypeAs<BankAccountApiDriver, IBankAccountDriver>();
            _container.RegisterTypeAs<BankFeesDriver, IBankFeesDriver>();
            _container.RegisterTypeAs<CustomerRegistrationApiDriver, ICustomerRegistrationDriver>();
            _container.RegisterTypeAs<ActivityApiDriver, IActivityDriver>();

        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}