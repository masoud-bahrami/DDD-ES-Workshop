using System.Text;
using Bank.Account.API;
using BankAccount.BankFees;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace BankAccount.AcceptanceTests.Drivers.AccountManager;

internal interface IBankFeesDriver
{
    Task SetSmsFees(Table amount);
}

public class BankFeesDriver : IBankFeesDriver
{
    private readonly HttpClient _httpClient;

    public BankFeesDriver()
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

    public async Task SetSmsFees(Table amount)
    {
        var setBankFeesCommand = new SetBankFeesCommand
        {
            SmsFees= decimal.Parse(amount.Rows[0]["Sms"]),
            Charges = decimal.Parse(amount.Rows[0]["Charges"]),
        };

        var httpResponseMessage = await _httpClient.PostAsync("api/bankfees",
            new StringContent(JsonConvert.SerializeObject(setBankFeesCommand)
            , Encoding.UTF8,"application/json")
            );

        httpResponseMessage.EnsureSuccessStatusCode();
    }
}