using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankAccount.BankFees.Bootstrapper
{
    public class BankFeesBootstrapper

    {
        public static void Run(IServiceCollection serviceCollection)
        {

            // bank fees BC

            serviceCollection.AddSingleton<BankFeesDbContext>();
            serviceCollection.AddTransient<IBankFeesServices, BankFeesServices>();
            
            serviceCollection.AddSingleton(
                sp =>
                {
                    var connection = new SqliteConnection("Data Source=:memory:");

                    connection.Open();

                    return new DbContextOptionsBuilder<BankFeesDbContext>()
                        .UseSqlite(connection).Options;
                });

        }
    }
}