using BankAccount.BankFees.Bootstrapper;
using BankAccounting.Account.Bootstrapper;
using BankAccount.CustomerManagement.Bootstrapper;

namespace Bank.Account.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        AccountBootstrapper.Run(builder.Services);
        BankFeesBootstrapper.Run(builder.Services);
        CustomerManagementBootstrapper.Run(builder.Services);
            
        var app = builder.Build();
            
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
            
        AccountBootstrapper.Migrate(app.Services);
        BankFeesBootstrapper.Migrate(app.Services);
        CustomerManagementBootstrapper.Migrate(app.Services);

        app.Run();
    }
}