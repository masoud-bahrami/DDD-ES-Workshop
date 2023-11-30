using BankAccount.ApplicationServices;
using BankAccount.ApplicationServices.Query;
using BankAccount.BankFees;
using BankAccount.Domain.Accounts.Repository;
using BankAccount.Domain.Accounts.Services;
using BankAccount.Domain.Services;
using BankAccount.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Zero.Dispatcher.Command;
using Zero.Dispatcher.Query;

namespace Bank.Account.API
{
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

            builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            // db context(s)
            builder.Services.AddSingleton<BankAccountDbContext>();

            builder.Services.AddSingleton(
                sp =>
                {
                    var connection = new SqliteConnection("Data Source=:memory:");

                    connection.Open();

                    return new DbContextOptionsBuilder<BankAccountDbContext>()
                        .UseSqlite(connection).Options;
                });

            // register query handlers
            builder.Services.AddTransient<IWantToHandleQuery<GetAccountBalanceQuery, decimal>, GetAccountBalanceQueryHandler>();

            // register command handlers
            builder.Services.AddScoped<IWantToHandleCommand<OpenBankAccountCommand>, OpenBankAccountCommandHandler>();

            builder.Services.AddTransient<IAccountDomainService, AccountDomainService>(sp => new AccountDomainService(10000));
            builder.Services.AddTransient<IAccountIdGeneratorDomainService, AccountIdGeneratorDomainService>();
            
            
            builder.Services.AddTransient<IBankFeesServices, BankFeesServices>();


            // register repositories
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

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


            var bankAccountDbContext = app.Services.GetService<BankAccountDbContext>();
            bankAccountDbContext.Database.EnsureCreated();
            bankAccountDbContext.Database.Migrate();

            app.Run();
        }
    }
}