using System.Text;
using Bank.Account.API;
using BankAccount.BankFees;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace BankAccount.AcceptanceTests.Drivers.AccountManager;

public interface IBankFeesDriver
{
    Task SetFees(Table amount);
}

public class BankFeesDriver : IBankFeesDriver
{
    private readonly HttpClient _httpClient;

    public BankFeesDriver(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SetFees(Table amount)
    {
        var setBankFeesCommand = new SetBankFeesCommand
        {
            SmsFees= decimal.Parse(amount.Rows[0]["Sms"]),
            Charges = decimal.Parse(amount.Rows[0]["Charges"]),
        };

        var httpResponseMessage = await _httpClient.PostAsync("api/bankfees", Content(setBankFeesCommand));

        httpResponseMessage.EnsureSuccessStatusCode();
    }

    private static StringContent Content(SetBankFeesCommand setBankFeesCommand)
    {
        return new StringContent(JsonConvert.SerializeObject(setBankFeesCommand)
            , Encoding.UTF8,"application/json");
    }
}