using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Restaurant.KitchenManager.API.Repositories.Pizzas;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Restaurant.KitchenManager.API.Models;
using System.IO;
using System.Net;

namespace Restaurant.KitchenManager.API.Functions.Pizzas
{
    public class CreatePizza
    {
        private readonly ILogger<CreatePizza> _logger;
        private readonly IConfiguration _config;
        private readonly IPizzaRepository _pizzaRepository;

        public CreatePizza(
            ILogger<CreatePizza> logger,
            IConfiguration config,
            IPizzaRepository pizzaRepository)
        {
            _logger = logger;
            _config = config;
            _pizzaRepository = pizzaRepository;
        }

        [FunctionName(nameof(CreatePizza))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "pizza")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation("Creating pizza.");
                var requestData = await new StreamReader(req.Body).ReadToEndAsync();
                var responsePizza = JsonConvert.DeserializeObject<Pizza>(requestData);

                var name = responsePizza.Name;
                if(string.IsNullOrEmpty(name))
                {
                    _logger.LogError($"Name is not supplied");
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                Pizza oldPizza = null;
                try
                {
                    oldPizza = await _pizzaRepository.GetPizzaByName(name);
                }
                catch(CosmosException cex) when(cex.StatusCode == HttpStatusCode.NotFound)
                {
                }

                if(oldPizza == null)
                {
                    await _pizzaRepository.CreatePizza(responsePizza);

                    result = new CreatedResult("", responsePizza);
                }
                else
                {
                    _logger.LogError($"Duplicate Pizza Error: Exception thrown: Pizza is already exists!");
                    result = new ConflictResult();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
