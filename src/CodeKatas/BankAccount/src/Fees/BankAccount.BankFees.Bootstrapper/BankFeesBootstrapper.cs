using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankAccount.BankFees.Bootstrapper
{
    public class BankFeesBootstrapper

    {
        public static void Run(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<BankFeesDbContext>();
            serviceCollection.AddSingleton(
                sp =>
                {
                    var connection = new SqliteConnection("Data Source=:memory:");

                    connection.Open();

                    return new DbContextOptionsBuilder<BankFeesDbContext>()
                        .UseSqlite(connection).Options;
                });

            serviceCollection.AddTransient<IBankFeesServices, BankFeesServices>();

        }

        public static void Migrate(IServiceProvider serviceProvider)
        {
            var bankFeesDbContext = serviceProvider.GetService<BankFeesDbContext>();
            bankFeesDbContext.Database.EnsureCreated();
            bankFeesDbContext.Database.Migrate();
        }
    }
}