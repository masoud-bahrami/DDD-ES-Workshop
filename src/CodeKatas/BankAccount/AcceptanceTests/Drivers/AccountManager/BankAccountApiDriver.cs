namespace BankAccount.AcceptanceTests.Drivers.AccountManager;

public class BankAccountApiDriver : IBankAccountDriver
{
    // test double
    // 
    private readonly HttpClient _httpClient;

    public BankAccountApiDriver(HttpClient httpClient)
    {
        _httpClient = HttpClientFactory.HttpClientOfAccountManagement();

        //_httpClient = httpClient;
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