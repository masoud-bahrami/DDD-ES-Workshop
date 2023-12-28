namespace BankAccount.CustomerManagement.Host;

public class Program
{
    public static async Task Main(string[] args)
    {
        var webHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        await webHost.StartAsync();
    }
}