using BankAccount.BankFees.Bootstrapper;
using BankAccounting.Account.Bootstrapper;

namespace Bank.Account.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Host
        // Getaway

        // 
        var customerManagementHost = CreateCustomerManagementWebHostBuilder(args).Build();
        var accountManagementHost = CreateAccountManagementHostBuilder(args).Build();
        var bankFeesHost = CreateBankFeeHostBuilder(args).Build();

        await Task.WhenAll(customerManagementHost.StartAsync(),
            accountManagementHost.StartAsync(),
            bankFeesHost.StartAsync());
    }
    
    private static IHostBuilder CreateCustomerManagementWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                       .UseUrls("http://*:5000", "https://*:5001")
                       .UseStartup<BankAccount.CustomerManagement.Host.Startup>();
            });

    private static IHostBuilder CreateAccountManagementHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseUrls("http://*:5002", "https://*:5003")
                    .UseStartup<Bank.AccountManagement.Api.Startup>();
            });

    private static IHostBuilder CreateBankFeeHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseUrls("http://*:5004", "https://*:5005")
                    .UseStartup<BankAccount.BankFees.Host.Startup>();
            });
}