using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Restaurant.KitchenManager.API.Models;
using Restaurant.KitchenManager.API.Repositories.Toppings;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace Restaurant.KitchenManager.API.Functions.Toppings
{
    public class CreateTopping
    {
        private readonly ILogger<CreateTopping> _logger;
        private readonly IConfiguration _config;
        private readonly IToppingRepository _toppingRepository;

        public CreateTopping(
            ILogger<CreateTopping> logger,
            IConfiguration config,
            IToppingRepository toppingRepository)
        {
            _logger = logger;
            _config = config;
            _toppingRepository = toppingRepository;
        }

        [FunctionName(nameof(CreateTopping))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "topping")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation("Creating Topping.");
                var requestData = await new StreamReader(req.Body).ReadToEndAsync();
                var responseTopping = JsonConvert.DeserializeObject<Topping>(requestData);

                var name = responseTopping.Name;
                var kind = responseTopping.Kind;
                if(string.IsNullOrEmpty(name))
                {
                    _logger.LogError($"Name is not supplied");
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                Topping oldTopping = null;
                try
                {
                    oldTopping = await _toppingRepository.GetToppingByName(name);
                }
                catch(CosmosException cex) when(cex.StatusCode == HttpStatusCode.NotFound)
                {
                }

                if(oldTopping == null)
                {
                    await _toppingRepository.CreateTopping(responseTopping);

                    result = new CreatedResult("", responseTopping);
                }
                else
                {
                    _logger.LogError($"Duplicate Topping Error: Exception thrown: Topping is already exists!");
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
