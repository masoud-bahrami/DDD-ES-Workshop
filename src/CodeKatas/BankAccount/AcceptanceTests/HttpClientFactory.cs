using Microsoft.AspNetCore.Mvc.Testing;

namespace BankAccount.AcceptanceTests;

public class HttpClientFactory
{
    private static HttpClient? _accountManagementClient = null;

    public static HttpClient HttpClientOfAccountManagement()
    {
        Initial();

        return _accountManagementClient;

        void Initial()
        {
            if (_accountManagementClient is null)
            {
                var application = new WebApplicationFactory<Bank.AccountManagement.Api.Startup>()
                    .WithWebHostBuilder(builder => { builder.ConfigureServices(services => { }); });

                _accountManagementClient = application.CreateClient();
            }
        }
    }


    public static HttpClient HttpClientOfBankFees()
    {
        Initial();

        return _accountManagementClient;

        void Initial()
        {
            if (_accountManagementClient is null)
            {
                var application = new WebApplicationFactory<Bank.AccountManagement.Api.Startup>()
                    .WithWebHostBuilder(builder => { builder.ConfigureServices(services => { }); });

                _accountManagementClient = application.CreateClient();
            }
        }
    }
}