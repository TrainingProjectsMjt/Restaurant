using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Restaurant.KitchenManager.API.Repositories.Toppings;
using System.Threading.Tasks;

namespace Restaurant.KitchenManager.API.Functions.Toppings
{
    public class GetToppingByName
    {
        private readonly ILogger<GetToppingByName> _logger;
        private readonly IConfiguration _config;
        private readonly IToppingRepository _toppingRepository;

        public GetToppingByName(
            ILogger<GetToppingByName> logger,
            IConfiguration config,
            IToppingRepository toppingRepository)
        {
            _logger = logger;
            _config = config;
            _toppingRepository = toppingRepository;
        }

        [FunctionName(nameof(GetToppingByName))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "topping/name/{name}")] HttpRequest req,
            string name)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation($"Getting Topping Name: {name}");
                if(string.IsNullOrEmpty(name))
                {
                    _logger.LogError($"Id is not supplied");
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                var topping = await _toppingRepository.GetToppingByName(name);
                if(topping == null)
                {
                    _logger.LogError($"Could not find Topping Name: {name}");
                    throw new CosmosException("Topping not found!", HttpStatusCode.NotFound, 404, "", 0.0);
                }
                result = new OkObjectResult(topping);
            }
            catch(CosmosException cex) when(cex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError($"Could not find Topping Name: {name}");
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
