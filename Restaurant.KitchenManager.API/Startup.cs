using System;
using System.IO;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Restaurant.KitchenManager.API;
using Restaurant.KitchenManager.API.Repositories.Pizzas;
using Restaurant.KitchenManager.API.Repositories.Toppings;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Restaurant.KitchenManager.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddFilter(level => true);
            });

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            builder.Services.AddTransient<IToppingRepository, ToppingRepository>();
            builder.Services.AddTransient<IPizzaRepository, PizzaRepository>();

            var connectionString = config["CosmosDBConnectionString"];
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Please specify a valid CosmosDBConnectionString in the appSettings.json file or your Azure Functions settings.");
            }

            var cosmosClientOptions = new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Direct
            };

            builder.Services.AddSingleton((s) => new CosmosClient(connectionString, cosmosClientOptions));
        }
    }
}
