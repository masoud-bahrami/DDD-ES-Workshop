using Bank.Account.Domain.Contracts.Commands;
using BankAccount.ApplicationServices;
using BankAccount.Domain.Accounts.Repository;
using BankAccount.Domain.Accounts.Services;
using BankAccount.Domain.Services;
using BankAccount.Infrastructure;
using BankAccount.Reporting.ApplicationServices;
using BankAccount.Reporting.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zero.DataBase;
using Zero.Dispatcher.Command;
using Zero.Dispatcher.CommandPipeline;
using Zero.Dispatcher.CommandPipeline.Stages;
using Zero.Dispatcher.Query;
using Zero.Dispatcher.QueryPipeline;
using Zero.EventSourcing;
using Zero.EventSourcing.DispatcherPipeline;
using Zero.EventSourcing.EventStoreDb;
using Zero.EventSourcing.InMemoryEventStore;
using Zero.EventSourcing.InMemoryEventStore.Quantum.Core;
using Zero.EventSourcing.Projection;

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
            serviceCollection.AddSingleton<IWantToHandleQuery<GetAccountBalanceQuery, decimal>, GetAccountBalanceQueryHandler>();

            // register command handlers
            serviceCollection.AddSingleton<IWantToHandleCommand<OpenBankAccountCommand>, OpenBankAccountCommandHandler>();

            serviceCollection.AddTransient<IAccountDomainService, AccountDomainService>(sp => new AccountDomainService(10000));
            serviceCollection.AddTransient<IAccountIdGeneratorDomainService, AccountIdGeneratorDomainService>();

            //builder.Services.AddTransient<IBankFeesDomainService, BankFeesDomainService>();

            // register repositories
            serviceCollection.AddSingleton<IAccountRepository, AccountRepository>();


            // Acl

            serviceCollection.AddTransient<IBankFeesAcl, BankFeesAcl>();


            serviceCollection.AddSingleton<IDbContextInterceptor, DbContextInterceptor>();

            serviceCollection.AddSingleton<AccountProjector>();
            serviceCollection.AddSingleton<IProjectorsLedger>(sp => new ProjectorsLedger(typeof(AccountProjector).Assembly));

            //serviceCollection.AddSingleton<IEventStore, InMemoryEventStore>();
            serviceCollection.AddSingleton<IEventStore, EventStoreDbEventStore>();
            serviceCollection.AddSingleton<EventStoreDbConfig>(sp => new EventStoreDbConfig());

            serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();


            serviceCollection.AddSingleton<AppendToEventStoreStage>();
            serviceCollection.AddSingleton<ProjectDomainEventsStage>(sp =>
                new ProjectDomainEventsStage(sp.GetService<IProjectorsLedger>(), sp));
            serviceCollection.AddSingleton<LogPipelineStage>();
            serviceCollection.AddSingleton<CorrelationIdStage>();
            serviceCollection.AddSingleton<HandlingCommandStage>();
            serviceCollection.AddSingleton<EfUnitOfWorkStage>();
            serviceCollection.AddSingleton<CallEventListenersStage>();
            serviceCollection.AddSingleton<IAmACommandPipeline>(sp =>
            {
                var logStage = sp.GetService<LogPipelineStage>();
                var correlationIdStage = sp.GetService<CorrelationIdStage>();
                var handlingCommandStage = sp.GetService<HandlingCommandStage>();
                var appendToEventStore = sp.GetService<AppendToEventStoreStage>();
                var projectDomainEventsStage = sp.GetService<ProjectDomainEventsStage>();
                var efUnitOfWorkStage = sp.GetService<EfUnitOfWorkStage>();
                var callEventListenersStage = sp.GetService<CallEventListenersStage>();

                return HeyPipeline.IWant()
                                     .ToDefineAPipeline()
                                             .WithStarterStage(logStage)
                                             .WithSuccessor(correlationIdStage)
                                             .WithSuccessor(handlingCommandStage)

                                             .WithSuccessor(appendToEventStore)
                                             .WithSuccessor(projectDomainEventsStage)

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