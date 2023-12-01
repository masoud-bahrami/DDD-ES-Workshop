using Bank.Account.API;
using BankAccount.AcceptanceTests.Drivers.AccountManager;
using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;

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
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                    });
                });

            _httpClient = application.CreateClient();

            _container.RegisterInstanceAs(_httpClient, typeof(HttpClient));
            
            _container.RegisterTypeAs<BankAccountApiDriver,IBankAccountDriver>();
            _container.RegisterTypeAs<BankFeesDriver,IBankFeesDriver>();

        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}