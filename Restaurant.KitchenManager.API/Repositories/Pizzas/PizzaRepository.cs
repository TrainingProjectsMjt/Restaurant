using System;
using System.Collections.Generic;
using System.Text;
using Restaurant.KitchenManager.API.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net;

namespace Restaurant.KitchenManager.API.Repositories.Pizzas
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly IConfiguration _config;
        private readonly Container _pizzaContainer;

        public PizzaRepository(
            CosmosClient cosmosClient,
            IConfiguration config)
        {
            _cosmosClient = cosmosClient;
            _config = config;
            _pizzaContainer = _cosmosClient.GetContainer(_config["DatabaseName"], _config["PizzasContainerName"]);
        }

        public async Task CreatePizza(Pizza pizza)
        {
            var itemRequestOptions = new ItemRequestOptions()
            {
                EnableContentResponseOnWrite = false
            };

            await _pizzaContainer.CreateItemAsync(
                pizza,
                new PartitionKey(pizza.PizzaId),
                itemRequestOptions);
        }

        public async Task DeletePizza(string id, string pizzaId)
        {
            var itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _pizzaContainer.DeleteItemAsync<Topping>(
                id,
                new PartitionKey(pizzaId),
                itemRequestOptions);
        }

        public async Task<List<Pizza>> GetAllPizzas()
        {
            var toppings = new List<Pizza>();

            var itemsFeedIterator = _pizzaContainer.GetItemQueryIterator<Pizza>();

            while(itemsFeedIterator.HasMoreResults)
            {
                var orderResponse = await itemsFeedIterator.ReadNextAsync();
                toppings.AddRange(orderResponse.Resource);
            }

            return toppings;
        }

        public async Task<Pizza> GetPizzaById(string id)
        {
            var toppings = new List<Pizza>();

            var query = new QueryDefinition("SELECT * FROM Pizzas i WHERE i.id = @id")
                    .WithParameter("@id", id);
            var itemsFeedIterator = _pizzaContainer.GetItemQueryIterator<Pizza>(query);

            while(itemsFeedIterator.HasMoreResults)
            {
                var orderResponse = await itemsFeedIterator.ReadNextAsync();
                toppings.AddRange(orderResponse.Resource);
            }
            if(toppings.Any())
            {
                return toppings.First();
            }
            else
            {
                throw new CosmosException("Pizza not found!", HttpStatusCode.NotFound, 404, "", 0.0);
            }
        }

        public async Task<Pizza> GetPizzaByName(string name)
        {
            var toppings = new List<Pizza>();

            var query = new QueryDefinition("SELECT * FROM Pizzas i WHERE i.name = @name")
                    .WithParameter("@name", name);
            var itemsFeedIterator = _pizzaContainer.GetItemQueryIterator<Pizza>(query);

            while(itemsFeedIterator.HasMoreResults)
            {
                var orderResponse = await itemsFeedIterator.ReadNextAsync();
                toppings.AddRange(orderResponse.Resource);
            }
            if(toppings.Any())
            {
                return toppings.First();
            }
            else
            {
                throw new CosmosException("Pizza not found!", HttpStatusCode.NotFound, 404, "", 0.0);
            }
        }

        public async Task UpdatePizza(Pizza pizza)
        {
            var itemRequestOptions = new ItemRequestOptions()
            {
                EnableContentResponseOnWrite = false
            };

            await _pizzaContainer.UpsertItemAsync(
                pizza,
                new PartitionKey(pizza.PizzaId),
                itemRequestOptions);
        }

        public async Task UpdateToppingsInPizza(Pizza pizza)
        {
            var itemRequestOptions = new ItemRequestOptions()
            {
                EnableContentResponseOnWrite = false
            };

            await _pizzaContainer.UpsertItemAsync(
                pizza,
                new PartitionKey(pizza.PizzaId),
                itemRequestOptions);
        }
    }
}
