using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Restaurant.KitchenManager.UnitTests.Helpers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Restaurant.KitchenManager.API.Functions.Pizzas;
using Microsoft.Extensions.Configuration;
using Restaurant.KitchenManager.API.Repositories.Pizzas;
using Restaurant.KitchenManager.API.Models;
using System.Collections.Generic;

namespace Restaurant.KitchenManager.UnitTests.FunctionTests.Pizzas
{
    public class GetAllPizzasShould
    {
        private Mock<ILogger<GetAllPizzas>> _loggerMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IPizzaRepository> _pizzaRepositoryMock;
        private Mock<HttpRequest> _httpRequestMock;

        private GetAllPizzas _func;

        public GetAllPizzasShould()
        {
            _loggerMock = new Mock<ILogger<GetAllPizzas>>();
            _configMock = new Mock<IConfiguration>();
            _pizzaRepositoryMock = new Mock<IPizzaRepository>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new GetAllPizzas(
                _loggerMock.Object,
                _configMock.Object,
                _pizzaRepositoryMock.Object);
        }

        [Fact]
        public async Task Return200OnOk()
        {
            // Arrange
            var allPizzas = TestDataGenerator.GenerateAllPizzas();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(allPizzas));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.GetAllPizzas())
                .ReturnsAsync(() => allPizzas);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
            var okObjectResult = response as OkObjectResult;
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Throw500OnInternalServerError()
        {
            // Arrange
            var allPizzas = TestDataGenerator.GenerateAllPizzas();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(allPizzas));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.GetAllPizzas())
                .Throws(new Exception("Some error!"));

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCode.StatusCode);
        }
    }
}
