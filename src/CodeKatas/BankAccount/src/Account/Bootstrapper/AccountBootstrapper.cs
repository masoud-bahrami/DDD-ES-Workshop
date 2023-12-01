using Bank.Account.Domain.Contracts.Commands;
using BankAccount.ApplicationServices;
using BankAccount.ApplicationServices.Query;
using BankAccount.BankFees;
using BankAccount.Domain.Accounts.Repository;
using BankAccount.Domain.Accounts.Services;
using BankAccount.Domain.Services;
using BankAccount.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zero.Dispatcher.Command;
using Zero.Dispatcher.Query;

namespace BankAccounting.Account.Bootstrapper
{
    public class AccountBootstrapper
    {
        public static void Run(IServiceCollection serviceCollection)
        {

           serviceCollection.AddScoped<ICommandDispatcher, CommandDispatcher>();
           serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();

            // db context(s)
           serviceCollection.AddSingleton<BankAccountDbContext>();

           serviceCollection.AddSingleton(
                sp =>
                {
                    var connection = new SqliteConnection("Data Source=:memory:");

                    connection.Open();

                    return new DbContextOptionsBuilder<BankAccountDbContext>()
                        .UseSqlite(connection).Options;
                });

            // register query handlers
           serviceCollection.AddTransient<IWantToHandleQuery<GetAccountBalanceQuery, decimal>, GetAccountBalanceQueryHandler>();

            // register command handlers
           serviceCollection.AddScoped<IWantToHandleCommand<OpenBankAccountCommand>, OpenBankAccountCommandHandler>();

           serviceCollection.AddTransient<IAccountDomainService, AccountDomainService>(sp => new AccountDomainService(10000));
           serviceCollection.AddTransient<IAccountIdGeneratorDomainService, AccountIdGeneratorDomainService>();

            //builder.Services.AddTransient<IBankFeesDomainService, BankFeesDomainService>();




            // register repositories
           serviceCollection.AddScoped<IAccountRepository, AccountRepository>();
           serviceCollection.AddTransient<IBankFeesServices, BankFeesServices>();


            // Acl

           serviceCollection.AddTransient<IBankFeesAcl, BankFeesAcl>();
        }
    }
}