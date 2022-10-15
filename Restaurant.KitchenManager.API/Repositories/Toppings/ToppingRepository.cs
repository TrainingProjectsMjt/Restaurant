using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Restaurant.KitchenManager.API.Models;

namespace Restaurant.KitchenManager.API.Repositories.Toppings
{
    public class ToppingRepository : IToppingRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly IConfiguration _config;
        private readonly Container _toppingsContainer;

        public ToppingRepository(
            CosmosClient cosmosClient,
            IConfiguration config)
        {
            _cosmosClient = cosmosClient;
            _config = config;
            _toppingsContainer = _cosmosClient.GetContainer(_config["DatabaseName"], _config["ToppingsContainerName"]);
        }

        public async Task CreateTopping(Topping topping)
        {
            var itemRequestOptions = new ItemRequestOptions()
            {
                EnableContentResponseOnWrite = false
            };

            await _toppingsContainer.CreateItemAsync(
                topping,
                new PartitionKey(topping.ToppingId),
                itemRequestOptions);
        }

        public async Task DeleteTopping(string id, string toppingId)
        {
            var itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _toppingsContainer.DeleteItemAsync<Topping>(
                id,
                new PartitionKey(toppingId),
                itemRequestOptions);
        }

        public async Task<List<Topping>> GetAllToppings()
        {
            var toppings = new List<Topping>();

            var itemsFeedIterator = _toppingsContainer.GetItemQueryIterator<Topping>();

            while (itemsFeedIterator.HasMoreResults)
            {
                var response = await itemsFeedIterator.ReadNextAsync();
                toppings.AddRange(response.Resource);
            }

            return toppings;
        }

        public async Task<Topping> GetToppingById(string id)
        {
            var toppings = new List<Topping>();

            var query = new QueryDefinition("SELECT * FROM Toppings i WHERE i.id = @id")
                    .WithParameter("@id", id);
            var itemsFeedIterator = _toppingsContainer.GetItemQueryIterator<Topping>(query);

            while(itemsFeedIterator.HasMoreResults)
            {
                var response = await itemsFeedIterator.ReadNextAsync();
                toppings.AddRange(response.Resource);
            }
            if(toppings.Any())
            {
                return toppings.First();
            }
            else
            {
                throw new CosmosException("Topping not found!", HttpStatusCode.NotFound, 404, "", 0.0);
            }
        }

        public async Task<Topping> GetToppingByName(string name)
        {
            var toppings = new List<Topping>();

            var query = new QueryDefinition("SELECT * FROM Toppings i WHERE i.name = @name")
                    .WithParameter("@name", name);
            var itemsFeedIterator = _toppingsContainer.GetItemQueryIterator<Topping>(query);

            while(itemsFeedIterator.HasMoreResults)
            {
                var response = await itemsFeedIterator.ReadNextAsync();
                toppings.AddRange(response.Resource);
            }
            if(toppings.Any())
            {
                return toppings.First();
            }
            else
            {
                throw new CosmosException("Topping not found!", HttpStatusCode.NotFound, 404, "", 0.0);
            }
        }

        public async Task UpdateTopping(Topping topping)
        {
            var itemRequestOptions = new ItemRequestOptions()
            {
                EnableContentResponseOnWrite = false
            };

            await _toppingsContainer.UpsertItemAsync(
                topping,
                new PartitionKey(topping.ToppingId),
                itemRequestOptions);
        }
    }
}
