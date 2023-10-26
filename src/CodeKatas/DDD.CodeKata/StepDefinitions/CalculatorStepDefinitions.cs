using Microsoft.AspNetCore.Mvc.Testing;
using WebApplication2;

namespace DDD.CodeKata.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private readonly HttpClient _httpClient;

        public CalculatorStepDefinitions()
        {
            //var application = new WebApplicationFactory<Program>()
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                    });
                });

            _httpClient = application.CreateClient();

        }
        [Given(@"there is no existing account for Masoud")]
        public void GivenThereIsNoExistingAccountFor()
        {
        }

        [When(@"Masoud opens a new account with an initial deposit of \$(.*)")]
        public async Task WhenMasoudOpensANewAccountWithAnInitialDepositOf(decimal amount)
        {
            var httpResponseMessage = await _httpClient.PostAsync("/api/account/deposit/" + amount, null);
            if (httpResponseMessage.IsSuccessStatusCode is false)
            {
                var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new Exception(readAsStringAsync);
            }
        }

        [Then(@"a new account should be created with a balance of \$(.*) for Masoud")]
        public void ThenANewAccountShouldBeCreatedWithABalanceOfForMasoud(int p0)
        {
            throw new PendingStepException();
        }

    }
}