using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Restaurant.KitchenManager.API.Repositories.Toppings;

namespace Restaurant.KitchenManager.API.Functions.Toppings
{
    public class GetAllToppings
    {
        private readonly ILogger<GetAllToppings> _logger;
        private readonly IConfiguration _config;
        private readonly IToppingRepository _toppingRepository;

        public GetAllToppings(
            ILogger<GetAllToppings> logger,
            IConfiguration config,
            IToppingRepository toppingRepository)
        {
            _logger = logger;
            _config = config;
            _toppingRepository = toppingRepository;
        }

        [FunctionName(nameof(GetAllToppings))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "toppings")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation($"Getting Toppings.");
                var toppings = await _toppingRepository.GetAllToppings();
                result = new OkObjectResult(toppings);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(500);
            }

            return result;
        }
    }
}
