using System.Text;
using BankAccount.BankFees;
using BankAccount.BankFees.BAL;
using Newtonsoft.Json;

namespace BankAccount.AcceptanceTests.Drivers.BankFees;

public class BankFeesDriver : IBankFeesDriver
{
    private readonly HttpClient _httpClient;

    public BankFeesDriver(HttpClient httpClient)
    {
        //_httpClient = HttpClientFactory.HttpClientOfAccountManagement();
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