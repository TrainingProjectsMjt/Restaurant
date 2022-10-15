using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurant.KitchenManager.API.Functions.Toppings;
using Restaurant.KitchenManager.API.Models;
using Restaurant.KitchenManager.API.Repositories.Pizzas;

namespace Restaurant.KitchenManager.API.Functions.Pizzas
{
    public class UpdateToppingsInPizzaByPizzaName
    {
        private readonly ILogger<UpdateToppingsInPizzaByPizzaName> _logger;
        private readonly IConfiguration _config;
        private readonly IPizzaRepository _pizzaRepository;

        public UpdateToppingsInPizzaByPizzaName(
            ILogger<UpdateToppingsInPizzaByPizzaName> logger,
            IConfiguration config,
            IPizzaRepository pizzaRepository)
        {
            _logger = logger;
            _config = config;
            _pizzaRepository = pizzaRepository;
        }

        [FunctionName(nameof(UpdateToppingsInPizzaByPizzaName))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "pizza/{name}")] HttpRequest req,
            string name)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation($"Updating Pizza Name: {name}");
                if(string.IsNullOrEmpty(name))
                {
                    _logger.LogError($"Name is not supplied");
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                var requestData = await new StreamReader(req.Body).ReadToEndAsync();
                var newPizza = JsonConvert.DeserializeObject<Pizza>(requestData);

                var oldPizza = await _pizzaRepository.GetPizzaByName(name);
                newPizza.Id = oldPizza.Id;
                newPizza.PizzaId = oldPizza.PizzaId;
                newPizza.ToppingNames = oldPizza.ToppingNames;

                await _pizzaRepository.UpdatePizza(newPizza);

                result = new OkObjectResult(newPizza);
            }
            catch(CosmosException cex) when(cex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError($"Could not find Pizza Name: {name}");
                result = new NotFoundResult();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(500);
            }

            return result;
        }
    }
}
