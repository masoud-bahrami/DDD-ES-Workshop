using Bank.AccountManagement.Api;

var webHost = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });
await webHost.StartAsync();