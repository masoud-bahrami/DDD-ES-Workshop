using BankAccount.CustomerManagement.DbContext;
using BankAccount.CustomerManagement.Domain;
using BankAccount.CustomerManagement.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BankAccount.CustomerManagement.Bootstrapper
{
    public class CustomerManagementBootstrapper
    {
        public static void Run(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICustomerServices, CustomerServices>();
            serviceCollection.AddSingleton<CustomerDbContext>();
            
            serviceCollection.AddSingleton< ICustomerIdDomainService ,CustomerIdDomainService >();

            serviceCollection.AddSingleton(
                sp =>
                {
                    var connection = new SqliteConnection("Data Source=:memory:");

                    connection.Open();

                    return new DbContextOptionsBuilder<CustomerDbContext>().UseSqlite(connection).Options;
                });
        }

        public static void Migrate(IServiceProvider appServices)
        {
            var customerDbContext = appServices.GetService<CustomerDbContext>();
            customerDbContext.Database.EnsureCreated();
            customerDbContext.Database.Migrate();
        }
    }
}
