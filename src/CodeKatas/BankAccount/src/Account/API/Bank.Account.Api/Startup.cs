
using BankAccounting.Account.Bootstrapper;

namespace Bank.AccountManagement.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Add services to the container.
        serviceCollection.AddSwaggerGen();

        serviceCollection.AddControllers()
            .AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        serviceCollection.AddEndpointsApiExplorer();

        AccountBootstrapper.Run(serviceCollection);
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        AccountBootstrapper.Migrate(app.ApplicationServices);

        // Configure the HTTP request pipeline.
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}