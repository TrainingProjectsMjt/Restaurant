using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Restaurant.KitchenManager.API.Repositories.Toppings;
using Restaurant.KitchenManager.API.Models;

namespace Restaurant.KitchenManager.API.Functions.Toppings
{
    public class UpdateToppingById
    {
        private readonly ILogger<UpdateToppingById> _logger;
        private readonly IConfiguration _config;
        private readonly IToppingRepository _toppingRepository;

        public UpdateToppingById(
            ILogger<UpdateToppingById> logger,
            IConfiguration config,
            IToppingRepository toppingRepository)
        {
            _logger = logger;
            _config = config;
            _toppingRepository = toppingRepository;
        }

        [FunctionName(nameof(UpdateToppingById))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "topping/{name}")] HttpRequest req,
            string name)
        {
            IActionResult result;

            try
            {
                _logger.LogInformation($"Updating Topping Name: {name}");
                if(string.IsNullOrEmpty(name))
                {
                    _logger.LogError($"Name is not supplied");
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                var requestData = await new StreamReader(req.Body).ReadToEndAsync();
                var newTopping = JsonConvert.DeserializeObject<Topping>(requestData);

                var oldTopping = await _toppingRepository.GetToppingByName(name);
                newTopping.Id = oldTopping.Id;
                newTopping.ToppingId = oldTopping.ToppingId;

                await _toppingRepository.UpdateTopping(newTopping);

                result = new OkObjectResult(newTopping);
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
