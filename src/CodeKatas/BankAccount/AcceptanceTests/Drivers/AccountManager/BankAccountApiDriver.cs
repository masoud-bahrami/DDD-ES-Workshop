
using Bank.Account.API;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BankAccount.AcceptanceTests.Drivers.AccountManager;

public class BankAccountApiDriver : IBankAccountDriver
{
    readonly HttpClient _httpClient;

    public BankAccountApiDriver()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                });
            });

        _httpClient = application.CreateClient();

    }
    public async Task OpenBank(string owner, decimal initialAmount)
    {
        var httpResponseMessage = await _httpClient.PostAsync($"api/accounts/{owner}/open/{initialAmount}", null);

        await EnsureSuccess(httpResponseMessage);
    }

    public async Task AssertThatOwnerHasAnAccountWithInitialBalance(string owner, decimal expectedAmount)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"api/accounts/{owner}/Balance");

        await EnsureSuccess(httpResponseMessage);

        var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();

        var actualBalance = decimal.Parse(readAsStringAsync);

        actualBalance
            .Should()
            .Be(expectedAmount);

    }

    private static async Task EnsureSuccess(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.IsSuccessStatusCode is false)
        {
            throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}