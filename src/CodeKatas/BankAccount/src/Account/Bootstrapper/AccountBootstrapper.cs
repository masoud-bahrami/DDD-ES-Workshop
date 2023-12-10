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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Zero.DataBase;
using Zero.Dispatcher.Command;
using Zero.Dispatcher.CommandPipeline;
using Zero.Dispatcher.CommandPipeline.Stages;
using Zero.Dispatcher.Query;
using Zero.Dispatcher.QueryPipeline;

namespace BankAccounting.Account.Bootstrapper
{
    public class AccountBootstrapper
    {
        public static void Run(IServiceCollection serviceCollection)
        {

           serviceCollection.AddScoped<ICommandDispatcher, CommandDispatcher>();
           serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();
            
           serviceCollection.AddSingleton<ZeroDbContext, BankAccountDbContext>();
           
           serviceCollection.AddSingleton(
                sp =>
                {
                    var connection = new SqliteConnection("Data Source=:memory:");

                    connection.Open();

                    return new DbContextOptionsBuilder<ZeroDbContext>()
                        .UseSqlite(connection).Options;
                });

            // register query handlers
           serviceCollection.AddSingleton<IWantToHandleQuery<GetAccountBalanceAmAQuery, decimal>, GetAccountBalanceQueryHandler>();

            // register command handlers
           serviceCollection.AddSingleton<IWantToHandleCommand<OpenBankAccountCommand>, OpenBankAccountCommandHandler>();

           serviceCollection.AddTransient<IAccountDomainService, AccountDomainService>(sp => new AccountDomainService(10000));
           serviceCollection.AddTransient<IAccountIdGeneratorDomainService, AccountIdGeneratorDomainService>();

           //builder.Services.AddTransient<IBankFeesDomainService, BankFeesDomainService>();
           
            // register repositories
           serviceCollection.AddSingleton<IAccountRepository, AccountRepository>();
           serviceCollection.AddTransient<IBankFeesServices, BankFeesServices>();


            // Acl

           serviceCollection.AddTransient<IBankFeesAcl, BankFeesAcl>();


           serviceCollection.AddSingleton<IDbContextInterceptor, DbContextInterceptor>();

           serviceCollection.AddSingleton<LogPipelineStage>();
           serviceCollection.AddSingleton<CorrelationIdStage>();
           serviceCollection.AddSingleton<HandlingCommandStage>();
           serviceCollection.AddSingleton<EfUnitOfWorkStage>();
           serviceCollection.AddSingleton<CallEventListenersStage>();
           serviceCollection.AddSingleton<IAmACommandPipeline>(sp =>
           {
               var logStage = sp.GetService<LogPipelineStage>();
               var correlationIdStage= sp.GetService<CorrelationIdStage>();
               var handlingCommandStage= sp.GetService<HandlingCommandStage>();
               var efUnitOfWorkStage= sp.GetService<EfUnitOfWorkStage>();
               var callEventListenersStage= sp.GetService<CallEventListenersStage>();





               return HeyPipeline.IWant()
                                    .ToDefineAPipeline()
                                            .WithStarterStage(logStage)
                                            .WithSuccessor(correlationIdStage)
                                            .WithSuccessor(handlingCommandStage)
                                            .WithSuccessor(efUnitOfWorkStage)
                                            .WithSuccessor(callEventListenersStage)
                                    .ThankYou();
           });


           serviceCollection.AddSingleton<RemoveSpaceAndNormalizeArabicCharactersOfStringFieldsOfQueryStage>();
           serviceCollection.AddSingleton<PipelineQueryDispatcher>();

           serviceCollection.AddSingleton<IAmQueryHandlerStage>(sp =>
           {
               var removeSpaceAndNormalizeArabicCharactersOfStringFieldsOfQueryStage = sp.GetService<RemoveSpaceAndNormalizeArabicCharactersOfStringFieldsOfQueryStage>();
               var pipelineQueryDispatcher = sp.GetService<PipelineQueryDispatcher>();

               return HeyQueryDispatcherPipeline.CreateAQueryDispatcher()
                   .WithStartingStage(removeSpaceAndNormalizeArabicCharactersOfStringFieldsOfQueryStage)
                   .ProceedBy(pipelineQueryDispatcher)
                   .ThankYou();
           });
        }

        public static void Migrate(IServiceProvider serviceProvider)
        {
            var bankAccountDbContext = serviceProvider.GetService<ZeroDbContext>();
            bankAccountDbContext.Database.EnsureCreated();
            bankAccountDbContext.Database.Migrate();

        }
    }
}