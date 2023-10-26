using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Controllers;

namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            builder.Services.AddScoped<ICommandHandler<DepositCommand>, DepositCommandHandler>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            builder.Services.AddSingleton<MyDbContext>();

            builder.Services.AddSingleton<DbContextOptions<MyDbContext>>(
                sp=>
            {
                var sqliteConnection = new SqliteConnection("Filename=:memory:");
                sqliteConnection.Open();

                var dbContextOptions = new DbContextOptionsBuilder<MyDbContext>()
                    .UseSqlite(sqliteConnection).Options;

                return dbContextOptions;
            });

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

            app.Run();
        }
    }


    public abstract class ICommandHandler<TCommand>
    {
        public abstract Task Handle(TCommand command);
    }

    public class CommandDispatcher : ICommandDispatcher
    {
        private IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        
        public async Task Dispatch<TCommand>(TCommand command)where TCommand : ICommand
        {
            var handler = serviceProvider.GetService<ICommandHandler<TCommand>>(); 
            
            await handler.Handle(command);
            
        }
    }
}