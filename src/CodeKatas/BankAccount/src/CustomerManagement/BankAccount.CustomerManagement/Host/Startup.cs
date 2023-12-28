using BankAccount.CustomerManagement.Bootstrapper;

namespace BankAccount.CustomerManagement.Host;

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

        CustomerManagementBootstrapper.Run(serviceCollection);

    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        CustomerManagementBootstrapper.Migrate(app.ApplicationServices);

        // Configure the HTTP request pipeline.
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        //app.Run();

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