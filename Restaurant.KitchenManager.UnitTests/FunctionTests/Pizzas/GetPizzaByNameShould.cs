using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Restaurant.KitchenManager.API.Functions.Pizzas;
using Restaurant.KitchenManager.API.Repositories.Pizzas;
using Restaurant.KitchenManager.UnitTests.Helpers;
using Xunit;

namespace Restaurant.KitchenManager.UnitTests.FunctionTests.Pizzas
{
    public class GetPizzaByNameShould
    {
        private Mock<ILogger<GetPizzaByName>> _loggerMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IPizzaRepository> _pizzaRepositoryMock;
        private Mock<HttpRequest> _httpRequestMock;

        private GetPizzaByName _func;

        public GetPizzaByNameShould()
        {
            _loggerMock = new Mock<ILogger<GetPizzaByName>>();
            _configMock = new Mock<IConfiguration>();
            _pizzaRepositoryMock = new Mock<IPizzaRepository>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new GetPizzaByName(
                _loggerMock.Object,
                _configMock.Object,
                _pizzaRepositoryMock.Object);
        }

        [Fact]
        public async Task Return200OnOkAsync()
        {
            // Arrange
            var pizza = TestDataGenerator.GenerateHawaiianPizza();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(pizza));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.GetPizzaByName(It.IsAny<string>()))
                .ReturnsAsync(() => pizza);

            // Act
            var response = await _func.Run(_httpRequestMock.Object, pizza.Name);

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
            var okObjectResult = response as OkObjectResult;
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Throw400OnBadRequestAsync()
        {
            // Arrange
            var pizzaName = "";

            // Act
            var response = await _func.Run(_httpRequestMock.Object, pizzaName);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = response as StatusCodeResult;
            Assert.Equal(400, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task Throw404OnNotFoundAsync()
        {
            // Arrange
            var pizza = TestDataGenerator.GenerateHawaiianPizza();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(pizza));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.GetPizzaByName(It.IsAny<string>()))
                .Throws(new CosmosException("Not found", HttpStatusCode.NotFound, 404, "someActivity", 0.0));

            // Act
            var response = await _func.Run(_httpRequestMock.Object, pizza.Id);

            // Assert
            Assert.Equal(typeof(NotFoundResult), response.GetType());
            var notFoundResult = (NotFoundResult)response;
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Throw500OnInternalServerErrorAsync()
        {
            // Arrange
            var pizza = TestDataGenerator.GenerateHawaiianPizza();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(pizza));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.GetPizzaByName(It.IsAny<string>()))
                .Throws(new Exception("Some error!"));

            // Act
            var response = await _func.Run(_httpRequestMock.Object, pizza.Name);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCode.StatusCode);
        }
    }
}
