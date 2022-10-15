using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Restaurant.KitchenManager.API.Functions.Toppings;
using Restaurant.KitchenManager.API.Repositories.Toppings;
using System.Threading.Tasks;
using Restaurant.KitchenManager.API.Repositories.Pizzas;

namespace Restaurant.KitchenManager.API.Functions.Pizzas
{
    public class GetAllPizzas
    {
        private readonly ILogger<GetAllPizzas> _logger;
        private readonly IConfiguration _config;
        private readonly IPizzaRepository _pizzaRepository;

        public GetAllPizzas(
            ILogger<GetAllPizzas> logger,
            IConfiguration config,
            IPizzaRepository pizzaRepository)
        {
            _logger = logger;
            _config = config;
            _pizzaRepository = pizzaRepository;
        }

        [FunctionName(nameof(GetAllPizzas))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "pizzas")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation($"Getting Pizzas.");
                var toppings = await _pizzaRepository.GetAllPizzas();
                result = new OkObjectResult(toppings);
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
