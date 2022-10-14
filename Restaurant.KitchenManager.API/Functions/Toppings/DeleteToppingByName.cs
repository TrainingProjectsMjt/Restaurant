using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Restaurant.KitchenManager.API.Repositories.Toppings;

namespace Restaurant.KitchenManager.API.Functions.Toppings
{
    public class DeleteToppingByName
    {
        private readonly ILogger<DeleteToppingByName> _logger;
        private readonly IConfiguration _config;
        private readonly IToppingRepository _toppingRepository;

        public DeleteToppingByName(
            ILogger<DeleteToppingByName> logger,
            IConfiguration config,
            IToppingRepository toppingRepository)
        {
            _logger = logger;
            _config = config;
            _toppingRepository = toppingRepository;
        }

        [FunctionName(nameof(DeleteToppingByName))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "topping/name/{name}")] HttpRequest req,
            string name)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation($"Deleting Topping Name: {name}");
                if(string.IsNullOrEmpty(name))
                {
                    _logger.LogError($"Name is not supplied");
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                var topping = await _toppingRepository.GetToppingByName(name);
                await _toppingRepository.DeleteTopping(topping.Id, topping.ToppingId);
                result = new AcceptedResult();
            }
            catch(CosmosException cex) when(cex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError($"Could not find Topping Id: {name}");
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
